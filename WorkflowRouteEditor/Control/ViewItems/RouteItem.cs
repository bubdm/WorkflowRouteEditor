using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WorkflowRouteEditor.Entities;
using WorkflowRouteEditor.Control.ViewItems;
using WorkflowRouteEditor.Control.Common;

namespace WorkflowRouteEditor.Control
{
    internal class RouteItem : ItemWraper<IRoute>, IHitTest, IDrawItem
    { 
        public RouteItem(IRoute item)
            : base(item)
        {
            
            Links = new ItemsCollection<LinkItem>();
            Links.CollectionChanged += Links_CollectionChanged;
            FontColor = Colors.Black;
            FlowDirection = FlowDirection.LeftToRight;
        }

        public ItemsCollection<LinkItem> Links { get; }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public Color FontColor { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FlowDirection FlowDirection { get; set; }
        public Point BindingPoint { get; protected set; }
        public Rect Bounds
        {
            get { return new Rect(BindingPoint.X - Width/2, BindingPoint.Y - Height/2, Width, Height); }
        }
        public bool HitTest(Point pt)
        {
            return Bounds.Contains(pt);
        }

        internal void SetBindingPoint(Point pt)
        {
            BindingPoint = pt;
        }
        internal void Render(DrawingContext dc, bool useCanvas = true)
        {
            DrawElement(
                dc,
                new SolidColorBrush(FillColor) {Opacity = 0.7 },
                new Pen(new SolidColorBrush(BorderColor) { Opacity = 0.7 }, 1), useCanvas);
        }
        internal void DrawElement(DrawingContext dc, Brush fill, Pen pen, bool useCanvas = true)
        {
            var center = BindingPoint;

            Links.ToList().ForEach(s => s.DrawElement(dc, fill, pen, (Width / 2) - 4 , useCanvas));

            dc.DrawEllipse(
                            fill,
                            pen,
                            center,
                            Width / 2,
                            Height / 2);

           
            if (string.IsNullOrEmpty(Name)) return;

            FormattedText ft = new FormattedText(
                Name,
                new CultureInfo("ru-Ru"),
                FlowDirection,
                new Typeface("Tahoma"),
                18,
                new SolidColorBrush(FontColor),
                1);

            dc.DrawText(ft, Point.Subtract(center, new Vector(ft.WidthIncludingTrailingWhitespace / 2, ft.Height / 2)));
        }

        private void Links_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset) return;

            var item = e.NewItems.OfType<LinkItem>().FirstOrDefault();
            if (item == null) return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Model.Next.Add(item.To.Model);
            }
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                Model.Next.Remove(item.To.Model);
            }
        }
    }

}
