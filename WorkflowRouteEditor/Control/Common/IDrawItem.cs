using System.Windows;
using System.Windows.Media;

namespace WorkflowRouteEditor.Control.Common
{
    interface IDrawItem
    {
        Rect Bounds { get; }
        Point BindingPoint { get; }
    }
}
