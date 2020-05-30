using System.Windows;
using System.Windows.Media;

namespace WorkflowRouteEditor.Control.Common
{
    interface IHitTest
    {
        bool HitTest(Point pt);
    }
}
