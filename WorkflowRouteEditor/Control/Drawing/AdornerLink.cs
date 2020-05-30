using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Control.ViewItems;

namespace WorkflowRouteEditor.Drawing
{
    internal class AdornerLink : Adorner, IAdornerDrawing
    {
        private readonly SolidColorBrush _brush;
        private readonly Pen _pen;
        private double _leftOffset;
        private double _topOffset;
        private bool _isDragRestricted;
        private readonly Rectangle _fakeRect;
        public AdornerLink(LinkItem source, Panel panel)
            : base(panel)
        {
            Source = source;
            var clip = source.Bounds;

            _fakeRect = new Rectangle
            {
                Fill = Brushes.Transparent,
                Width = clip.Width,
                Height = clip.Height
            };
            _brush = new SolidColorBrush(Colors.Red)
            {
                Opacity = 0.3
            };

            _pen = new Pen(Brushes.Red, 3)
            {
                DashStyle = DashStyles.Dash,
                DashCap = PenLineCap.Flat
            };
        }
        public (double X, double Y) Adge
        {
            get { return (10, 10);  }
        }
        public Point StartPoint { get; set; }
        public bool IsDragStarted { get; set; }
        public bool IsDragRestricted
        { 
            get => _isDragRestricted; 
            set { _isDragRestricted = value; UpdateLayout(); } 
        }
        public IDrawItem Source { get; }
        public double LeftOffset
        {
            get { return _leftOffset; }
            set
            {
                _leftOffset = value;
                UpdatePosition();
            }
        }
        public double TopOffset
        {
            get { return _topOffset; }
            set
            {
                _topOffset = value;
                UpdatePosition();
            }
        }
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();

            result.Children.Add(base.GetDesiredTransform(transform));
            //result.Children.Add(new RotateTransform(_angle, Source.BindingPoint.X, Source.BindingPoint.Y));

            return result;
        }

        protected override int VisualChildrenCount => 1;
        protected override Size MeasureOverride(Size constraint)
        {
            _fakeRect.Measure(constraint);
            return _fakeRect.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _fakeRect.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index) => _fakeRect;
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var to = Mouse.GetPosition(AdornedElement);
            to.Offset(5, -10);

            var item = ((LinkItem)Source).To ;

            var text = new FormattedText(
                item.Name,
                new CultureInfo("ru-Ru"),
                FlowDirection.LeftToRight,
                new Typeface("Tahoma"),
                18,
                new SolidColorBrush(item.FontColor),
                1);
            
            _brush.Color = item.FillColor;
          
            dc.DrawLine(_pen, Source.BindingPoint, to);

            dc.DrawRectangle(IsDragRestricted ? Brushes.Coral : _brush, null, new Rect(to, new Size(text.WidthIncludingTrailingWhitespace + 3, text.Height + 3)));

            to.Offset(2, 0);
            dc.DrawText(text, to);
        }
        private void UpdatePosition()
        {
            var adornerLayer = Parent as AdornerLayer;
            adornerLayer?.Update(AdornedElement);
        }
    }
}
