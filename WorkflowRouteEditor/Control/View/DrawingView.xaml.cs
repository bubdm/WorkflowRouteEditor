using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorkflowRouteEditor.Control;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Control.Drawing;
using WorkflowRouteEditor.Control.ViewItems;
using WorkflowRouteEditor.Drawing;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.View
{
    /// <summary>
    /// Interaction logic for DrawingControl.xaml
    /// </summary>
    public partial class DrawingView : UserControl
    {
        static DrawingView()
        {
            RouteItemsProperty = DependencyProperty.Register("RouteItems", typeof(ItemsCollection<RouteItem>), typeof(DrawingView),
              new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRouteItemsChanged)));

            SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(RouteItem), typeof(DrawingView),
              new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged)));
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingView control = (DrawingView)d;
            if (e.NewValue == null)
            {
                control.ClearValue(SelectedItemProperty);
            }
            else
            {
                control.SetValue(SelectedItemProperty, e.NewValue);
            }
            control.SelectedItem = e.NewValue as RouteItem;
        }
        private static void OnRouteItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingView control = (DrawingView)d;
            if (e.NewValue == null)
            {
                control.ClearValue(RouteItemsProperty);
            }
            else
            {
                control.SetValue(RouteItemsProperty, e.NewValue);
            }

            control.OnRouteItemsChanged(e.NewValue as ItemsCollection<RouteItem>);
        }

       

        public static readonly DependencyProperty RouteItemsProperty;
        public static readonly DependencyProperty SelectedItemProperty;
        private IAdornerDrawing _adorner;
        private readonly DrawingVisual visual;
        private ItemsCollection<RouteItem> _RouteItems;
        private RouteItem _selectedItem;

        public DrawingView()
        {
            InitializeComponent();
            visual = new DrawingVisual();

            Loaded += CanvasLayout_Loaded;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        internal ItemsCollection<RouteItem> RouteItems
        {
            get => _RouteItems;
            set
            {
                if (_RouteItems != null) _RouteItems.CollectionChanged -= OnRouteItemsCollectionChanged;

                _RouteItems = value;

                if (_RouteItems != null) _RouteItems.CollectionChanged += OnRouteItemsCollectionChanged;

                OnRouteItemsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        internal RouteItem SelectedItem
        {
            get =>_selectedItem; 
            set
            {
                if (_selectedItem != value)
                {
                    OnPropertyChanged(new DependencyPropertyChangedEventArgs(SelectedItemProperty, _selectedItem, value));
                }
                _selectedItem = value;
            }
        }
        private void OnRouteItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ReArrangeItems();

            InvalidateLayout();
        }
        private void OnRouteItemsChanged(ItemsCollection<RouteItem> items)
        {
            RouteItems = items;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            Point position = e.GetPosition(canvas);

            var source = GetItemAtPoint(position);
            if (source == null) return;

            e.Handled = true;

            // Initialize new Adorner object
            _adorner = source is RouteItem
                ? (IAdornerDrawing) new AdornerRoute(source as RouteItem, panel)
                : (IAdornerDrawing) new AdornerLink(source as LinkItem, panel);

            _adorner.StartPoint = position;

        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            Point position = e.GetPosition(canvas);
            var source = GetItemAtPoint(position);
            // Generate SelectionChanged
            SelectedItem = source as RouteItem;

            if (_adorner == null) return;
            
            DragFinished(source, false);

            e.Handled = true;
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (
                _adorner == null
                || e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            Point position = e.GetPosition(canvas);

            var (adgeX, adgeY) = _adorner.Adge;

            // To prevent out of Canvas adge rendering
            if (
                position.X - adgeX < 0 ||
                position.Y - adgeY < 0 ||
                position.X + adgeX > canvas.Width ||
                position.Y + adgeY > canvas.Height)
            {
                return;
            }

            if (
                _adorner.IsDragStarted == false &&
                ((Math.Abs(position.X - _adorner.StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance)
                || (Math.Abs(position.Y - _adorner.StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                // Fix start position for drag operation
                _adorner.StartPoint = position;
                _adorner.IsDragStarted = true;
                //Initialize renderring Layer
                var layer = AdornerLayer.GetAdornerLayer(panel);
                layer.Add(_adorner as Adorner);
            }

            if (_adorner.IsDragStarted)
            {
                if (_adorner.Source is LinkItem li)
                {
                    _adorner.IsDragRestricted = li.CanRedirected(GetItemAtPoint(position) as RouteItem) == false;
                }

                // Calculate result offset of drag operation to
                // render the Adorner object 
                _adorner.LeftOffset = position.X - _adorner.StartPoint.X;
                _adorner.TopOffset = position.Y - _adorner.StartPoint.Y;

                //Debug.WriteLine($"X: {_adorner.LeftOffset} Y:{_adorner.TopOffset}");
            }
            //Capture Mouse to drag operation
            CaptureMouse();
        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            canvas.ContextMenu = null;

            base.OnPreviewMouseRightButtonDown(e);
        }
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvas);
            var source = GetItemAtPoint(position);
            // Generate SelectionChanged
            SelectedItem = source as RouteItem;
            
            canvas.ContextMenu = CreateContextMenu(source);
            
            base.OnPreviewMouseRightButtonUp(e);
        }
        
        internal void InvalidateLayout()
        {
            if (
                   canvas.Clip == null 
                || double.IsNormal(canvas.Height) == false
                || double.IsNormal(canvas.Width) == false) return;

            canvas.Source = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, PixelFormats.Pbgra32); 

            using (var context = visual.RenderOpen())
            {
                context.DrawGeometry(new SolidColorBrush(Colors.WhiteSmoke) { Opacity = 0.3 }, null, canvas.Clip);

                if (RouteItems != null)
                {
                    RouteItems.ToList().ForEach(s => s.Render(context, false));
                }
            }

            ((RenderTargetBitmap)canvas.Source).Render(visual);
        }
        private void DragFinished(object source, bool cancel)
        {
            //Release Mouse
            Mouse.Capture(null);

            // Remove adorner object from rendering layer
            AdornerLayer.GetAdornerLayer(panel).Remove(_adorner as Adorner);

            if (_adorner.IsDragStarted && cancel == false && _adorner.IsDragRestricted == false)
            {
                //Move object to new position
                if (_adorner.Source is RouteItem rt)
                {
                    rt.SetBindingPoint( new Point(
                            _adorner.StartPoint.X + _adorner.LeftOffset, 
                            _adorner.StartPoint.Y + _adorner.TopOffset));
                }

                if(_adorner.Source is LinkItem li)
                {
                    if (source is RouteItem ri)
                    {
                        li.RedirectLink(ri);
                    }
                }

                InvalidateLayout();
            }

            _adorner = null;
        }
        private void ReArrangeItems()
        {
            if (RouteItems == null || canvas.Clip == null) return;

            const double radius = 100.0;
            double[] offsetX = {
                0,
                - 0.5 * radius,
                (0.5 * radius) * 2,
                (-0.7071 * radius) * 2,
                (0.7071 * radius) * 2,
                (-0.866 * radius) * 2,
                (0.866 * radius) * 2,
                -radius * 2,
                radius * 2,
                (-0.866 * radius) * 2,
                (0.866 * radius) * 2,
                (-0.7071 * radius) * 2,
                (0.7071 * radius) * 2,
                - 0.5 * radius,
                (0.5 * radius) * 2,
                0 };
            double[] offsetY = {
                radius + 5,
                0.866 * radius,
                0,
                0.7071 * radius,
                0,
                0.5 * radius,
                0,
                radius,
                0,
                0.5 * radius,
                0,
                 0.7071 * radius,
                0,
                0.866 * radius,
                0,
                5 };

            var center = canvas.Clip.Bounds.CenterLocation();

            var point = new Point(center.X, 0);
            int count = 0;

            foreach (RouteItem item in RouteItems)
            {
                if (item.BindingPoint.IsPointEmpty() == false) continue;

                point.Offset(offsetX[count], offsetY[count++]);
                item.SetBindingPoint( point);

            }
        }
        private void CanvasLayout_Loaded(object sender, RoutedEventArgs e)
        {

            ((Panel)Parent).SizeChanged += DrawingControl_SizeChanged;
            // Stretch Canvas to the Parent
            SetActualSize();

            ReArrangeItems();

            InvalidateLayout();
        }
        private void DrawingControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetActualSize();

            InvalidateLayout();
        }
        private IHitTest GetItemAtPoint(Point pt)
        {
            if (RouteItems == null) return null;

            var item = RouteItems.FirstOrDefault(r => r.HitTest(pt));
            if (item != null) return item;

            var link = RouteItems.SelectMany(r => r.Links, (r, li) => li).FirstOrDefault(l => l.HitTest(pt));
            return link;
        }
        private void SetActualSize()
        {
            canvas.Height = ActualHeight;
            canvas.Width = ActualWidth;
            canvas.Clip = new RectangleGeometry(new Rect(new Size(canvas.Width, canvas.Height)));
        }
        private ContextMenu CreateContextMenu(IHitTest source)
        {
            var menu = this.Resources[source == null ? "Empty" : source.GetType().Name] as ContextMenu;
            if (menu != null)
            {
                menu.DataContext = source != null ? (object)source : RouteItems;
            }
            return menu;
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var element = ((MenuItem)sender).DataContext as IHitTest;
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        { }
        private void Paste_Click(object sender, RoutedEventArgs e)
        { }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Move_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Arrange_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
