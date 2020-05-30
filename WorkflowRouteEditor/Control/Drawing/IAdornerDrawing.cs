using System.Windows;
using WorkflowRouteEditor.Control;
using WorkflowRouteEditor.Control.Common;

namespace WorkflowRouteEditor.Drawing
{
    interface IAdornerDrawing
    {
        (double X, double Y) Adge { get; }
        bool IsDragStarted { get; set; }
        Point StartPoint { get; set; }
        double LeftOffset { get; set; }
        double TopOffset { get; set; }
        IDrawItem Source { get; }
        bool IsDragRestricted {get; set;}
    }
}
