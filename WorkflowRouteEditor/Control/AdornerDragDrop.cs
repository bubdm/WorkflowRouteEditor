using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkflowRouteEditor.Control
{
    internal class AdornerDrawing : Adorner
    {
        private readonly Rectangle _rect;
        private readonly Pen _pen;
        private double _leftOffset;
        private double _topOffset;

        public AdornerDrawing(UIElement original) :
            base(original)
        {
            OriginalSource = original;

            _rect = new Rectangle
            {
                Width = OriginalSource.RenderSize.Width,
                Height = OriginalSource.RenderSize.Height
            };

            _rect.Fill = new VisualBrush(OriginalSource)
            {
                Opacity = 0.25
            };

            _pen = new Pen(new VisualBrush(OriginalSource), 1.5)
            {
                DashStyle = DashStyles.Dash,
                DashCap = PenLineCap.Flat
            };

        }
        public (double X, double Y) Adge 
        { 
            get { return (_rect.Height /2 , _rect.Width / 2); } 
        }
        public Point StartPoint { get; set; }
        public bool IsDragStarted { get; set; }
        public UIElement OriginalSource { get; }
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
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));

            return result;
        }

        protected override int VisualChildrenCount => 1;
        protected override Size MeasureOverride(Size constraint)
        {
            _rect.Measure(constraint);
            return _rect.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _rect.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index) => _rect;
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (AdornedElement is RouteItem rt)
            {
                Rect rect = rt.Bounds;
                rect.Offset(LeftOffset - Width/2, TopOffset - Height /2);

                rt.DrawElement(dc, rect, _rect.Fill, _pen);
            }
        }

        private void UpdatePosition()
        {
            var adornerLayer = Parent as AdornerLayer;
            adornerLayer?.Update(AdornedElement);
        }

    }
}

    

