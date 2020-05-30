using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WorkflowRouteEditor.Entities;


namespace WorkflowRouteEditor.Control
{
    /// <summary>
    /// Interaction logic for WFEditor.xaml
    /// </summary>
    public partial class WFEditor : UserControl
    {
        public WFEditor()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.Move;
            Debug.WriteLine("Control Enter");
        }

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.Move;
            Debug.WriteLine("Control Over");
        }

        public void AddItems(IEnumerable<Route> values)
        {
            canvas.AddElements(UIItemsFactory.Create(values));
            
        }
    }
}
