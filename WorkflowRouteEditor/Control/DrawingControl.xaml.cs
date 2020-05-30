using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorkflowRouteEditor.Control.Repository;
using WorkflowRouteEditor.Control.ViewItems;
using WorkflowRouteEditor.Control.ViewModel;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control
{
    public partial class DrawingControl : UserControl
    {
        static DrawingControl()
        {
            ItemsSourceProperty = ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<Route>), typeof(DrawingControl),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnItemsSourceChanged)));
            FileSourceProperty = ItemsSourceProperty = DependencyProperty.Register("FileSource", typeof(string), typeof(DrawingControl),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnFileSourceChanged)));
        }

        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty FileSourceProperty;
        private readonly List<IRoute> _items;
        private string _FileSource;
        private readonly MainViewModel _mainViewModel;
        public DrawingControl()
        {
            InitializeComponent();

            _items = new List<IRoute>();
            //DataContext = _mainViewModel = MainViewModel.Instance;

            //this.Loaded += DrawingControl_Loaded;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        public IEnumerable<IRoute> ItemsSource
        {
            get
            {
                return _items;
            }
            set
            {
                if (value == null)
                {
                    ClearValue(ItemsSourceProperty);
                }
                else
                {
                    SetValue(ItemsSourceProperty, (object)value);
                }
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        public string FileSource
        {
            get { return _FileSource; }
            set
            { 
                if(value == null)
                {
                    ClearValue(FileSourceProperty);
                }
                else
                {
                    SetValue(FileSourceProperty, value);
                }    
            }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl control = (DrawingControl)d;

            if (e.NewValue is IEnumerable<IRoute> enumerable)
            {
                control._items.Clear();
                control._items.AddRange(enumerable.OfType<IRoute>());
            }
            else
            {
                control._items.Clear();

            }

            control.OnItemsSourceChanged(e.NewValue as IEnumerable<IRoute>);
        }
        private static void OnFileSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl control = (DrawingControl)d;

            control._FileSource = e.NewValue?.ToString();

            control.OnFileSourceChanged(control._FileSource);
        }

        private void OnFileSourceChanged(string fileName)
        {
            if (DataContext is MainViewModel mv)
            {
                mv.LoadItems(fileName);
            }
        }
        private void OnItemsSourceChanged(IEnumerable<IRoute> values)
        {
            if (DataContext is MainViewModel mv)
            {
                mv.LoadItems(_items);
            }
        }
        //private void DrawingControl_Loaded(object sender, RoutedEventArgs e)
        //{

        //    //properties.DataContext = _mainViewModel.RouteViewModel;
        //    //drawing.DataContext = _mainViewModel.RouteItemViewModel;

        //    //drawing.RouteItems = _mainViewModel.RouteItemViewModel.Items;
        //}
    }
}
