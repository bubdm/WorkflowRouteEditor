using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkflowRouteEditor.WUIElements;

namespace WorkflowRouteEditor.Control
{

    public class VisualHostContainer : FrameworkElement
    {
        private readonly ItemsControl _parent;
        public VisualHostContainer(ItemsControl parent)
        {
            Children = new VisualCollection(this);
            AllowDrop = true;
            _parent = parent;
        }
        public void Display()
        {
            foreach (IWUIElement item in Children)
            {
                item?.Display();
            }
        }
        internal VisualCollection Children { get; }
        protected override int VisualChildrenCount => Children.Count;
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= VisualChildrenCount)
            {
                throw new ArgumentOutOfRangeException("Index -> GetVisualChild");
            }

            return Children[index];
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pt = e.GetPosition(this);
                VisualTreeHelper.HitTest(this, null, HitTestCallback, new PointHitTestParameters(pt));
            }
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Canvas cs = Parent as Canvas;
            return new Size(cs.Height, cs.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
       
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(WCircle))
            {
                var ddo = new DataObject("UI", result.VisualHit);
                DragDrop.DoDragDrop(_parent, ddo, DragDropEffects.Move);

                //((DrawingVisual)result.VisualHit).Opacity =
                //    ((DrawingVisual)result.VisualHit).Opacity == 1.0 ? 0.4 : 1.0;
            }

            // Stop the hit test enumeration of objects in the visual tree.
            return HitTestResultBehavior.Stop;
        }
        internal void SetSize()
        {
            Width  = _parent.ActualWidth;
            Height = _parent.ActualHeight;
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);

            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Hand);
            }

            e.Handled = true;
        }
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            //e.Effects = DragDropEffects.None;

            //if (e.Data.GetDataPresent("UI"))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.Move;
                Debug.WriteLine("Over");

            }
        }
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            base.OnQueryContinueDrag(e);
            //if(e.Action == DragAction.Continue)
            //{
            //    Debug.WriteLine("Continue");
            //}

            //if (e.Action == DragAction.Drop)
            //{
            //    Debug.WriteLine("Drop");
            //}
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            Debug.WriteLine("Leave");
        }
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            Debug.WriteLine("Enter");
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            Debug.WriteLine("Drop");
        }
    }
}