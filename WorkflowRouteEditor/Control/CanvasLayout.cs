using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WorkflowRouteEditor.Control
{


    public class CanvasLayout : Canvas
    {
        private AdornerDrawing _adorner;

        public CanvasLayout() :
            base()
        {
            Loaded += CanvasLayout_Loaded;
        }

        internal void AddElements(IEnumerable<RouteItem> values)
        {
            this.Children.Clear();

            foreach (var item in values)
            {
                this.Children.Add(item);
            }
        }
        internal void RemoveElements(IEnumerable<UIElement> values)
        {
            foreach (var item in values)
            {
                this.Children.Remove(item);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (e.OriginalSource.GetType() == typeof(RouteItem))
            {
                Point position = e.GetPosition(this);
                // To prevent Mouse "hang" error, release Adorner object if one constructed
                if (_adorner != null)
                {
                    AdornerLayer.GetAdornerLayer(_adorner.AdornedElement).Remove(_adorner);
                }
                // Initialize new Adorner object
                _adorner = new AdornerDrawing(e.OriginalSource as UIElement)
                {
                    StartPoint = position
                };

                e.Handled = true;
            }
        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (_adorner == null) return;

            DragFinished(false);

            e.Handled = true;
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (_adorner == null || e.LeftButton != MouseButtonState.Pressed) return;

            Point position = e.GetPosition(this);

            var adge = _adorner.Adge;

            // To prevent out of Canvas adge rendering
            if(
                position.X - adge.X < 0 ||
                position.Y - adge.Y < 0 ||
                position.X + adge.X > Width ||
                position.Y + adge.Y > Height )
            {
                return;
            }

            if (
                _adorner.IsDragStarted == false && 
                (( Math.Abs(position.X - _adorner.StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance) 
                || (Math.Abs(position.Y - _adorner.StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                // Fix start position for drag operation
                _adorner.StartPoint = position;
                _adorner.IsDragStarted = true;
                //Initialize renderring Layer
                var layer = AdornerLayer.GetAdornerLayer(_adorner.OriginalSource);
                layer.Add(_adorner);
            }
            
            if(_adorner.IsDragStarted)
            {
                // Calculate result offset of drag operation to
                // render the Adorner object 
                _adorner.LeftOffset = position.X - _adorner.StartPoint.X;
                _adorner.TopOffset = position.Y - _adorner.StartPoint.Y;
            }
            //Capture Mouse to drag operation
            CaptureMouse();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if(_adorner != null)
            {
                DragFinished(cancel: e.Key == Key.Escape);
                e.Handled = true;
            }
        }
        
        private void DragFinished(bool cancel)
        {
            //Release Mouse
            Mouse.Capture(null);
            // Remove adorner object from rendering layer
            AdornerLayer.GetAdornerLayer(_adorner.AdornedElement).Remove(_adorner);

            if (_adorner.IsDragStarted && cancel == false)
            {
                //Move object to new position
                if(_adorner.OriginalSource is RouteItem rt)
                {
                    rt.SetBindingPoint(new Point(
                            _adorner.StartPoint.X + _adorner.LeftOffset - _adorner.ActualWidth / 2,
                            _adorner.StartPoint.Y + _adorner.TopOffset - _adorner.ActualHeight / 2));
                }

                _adorner = null;

                InvalidateVisual();
            }
        }
        private void ReArrangeItems()
        {
            const double radius = 50.0;
            const double margin = 10.0;
            double offsetY = margin + radius;
            double offsetX = margin + radius * 2;

            var center = Clip.Bounds.CenterLocation();

            var point = new Point(center.X, offsetY / 2);

            foreach (RouteItem item in Children)
            {
                if (item.BindingPoint.IsPointEmpty() == false) continue;
                
                item.SetBindingPoint(point);
                point.Offset(point.X == center.X ? offsetX : point.X < center.X ? offsetX * 2 : -offsetX * 2, offsetY);
            }
        }
        private void CanvasLayout_Loaded(object sender, RoutedEventArgs e)
        {
            ((Panel)Parent).SizeChanged += CanvasLayout_SizeChanged;
            // Stretch Canvas to the Parent
            this.Height = ((Panel)Parent).ActualHeight;
            this.Width  = ((Panel)Parent).ActualWidth;
            this.Clip = new RectangleGeometry(new Rect(new Size(Width, Height)));

            ReArrangeItems();

        }
        private void CanvasLayout_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = e.NewSize.Height;
            this.Width = e.NewSize.Width;
            this.Clip = new RectangleGeometry(new Rect(e.NewSize));
        }
        
    }
}
