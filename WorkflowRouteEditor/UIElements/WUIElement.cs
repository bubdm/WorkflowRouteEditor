using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WorkflowRouteEditor.Control;

namespace WorkflowRouteEditor.WUIElements
{
    public interface IWUIElement
    {
        void Display();
    }


    internal class WUIElement<TEntity> : DrawingVisual, IWUIElement
    {
        protected WUIElement(VisualHostContainer parent, TEntity item)
        {
            Item = item;
            parent.Children.Add(this);
        }
        

        internal protected TEntity Item { get; }
        public void Display()
        {
            using DrawingContext dc = RenderOpen();

            OnRender(dc);
        }
        protected virtual void OnRender(DrawingContext dc)
        {

        }
    }
}
