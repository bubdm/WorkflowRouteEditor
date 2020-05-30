using System.Windows;
using System.Windows.Media;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Control.Drawing;

namespace WorkflowRouteEditor.Control.ViewItems
{
    internal class LinkItem : IHitTest, IDrawItem
    {
        public LinkItem(RouteItem from, RouteItem to)
        {
            To = to;
            From = from;
        }
        private double _offset;
        public Point BindingPoint
        {
            get { return From.BindingPoint; }
        }
        public Rect Bounds
        {
            get { return new Rect(From.BindingPoint, To.BindingPoint); }
        }
        public RouteItem To { get; internal protected set; }
        public RouteItem From { get; protected set; }
        public bool HitTest(Point pt)
        {
            (Point start, Point end) = GetFromTo(_offset);

            return start.LineHitTest(end, pt);
        }

        internal void DrawElement(DrawingContext dc, Brush fill, Pen pen, double offset, bool useCanvas = true)
        {
            _offset = offset;

            if (useCanvas) return;

            (Point start, Point end) = GetFromTo(offset);

            Geometry line = start.ArrowLineGeometry(end);

            pen.Thickness = 3;
            dc.DrawGeometry(fill, pen, line);
        }

        private (Point from, Point to) GetFromTo(double offset)
        {
            var from = From.BindingPoint;
            var to = To.BindingPoint;
            
            from.Offset(from.X > to.X ? -4 : 4, from.Y > to.Y ? 4 : -4);
            to.Offset(from.X > to.X ? -4 : 4, from.Y > to.Y ? 4 : -4);

            from = from.PointInLine(to, offset);
            to = to.PointInLine(from, offset);

            return (from, to);
        }
    }

}
