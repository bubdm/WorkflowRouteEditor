using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using WorkflowRouteEditor.Control;
using WorkflowRouteEditor.Control.Common;

namespace WorkflowRouteEditor.Drawing
{
    internal class AdornerRoute : Adorner, IAdornerDrawing
    {
        private readonly SolidColorBrush _brush;
        private readonly Pen _pen;
        private double _leftOffset;
        private double _topOffset;
        private readonly Rectangle _fakeRect;
        public AdornerRoute(RouteItem original, Panel panel) :
            base(panel)
        {
            Source = original;

            _brush = new SolidColorBrush(original.FillColor)
            {
                Opacity = 0.3
            };

            _pen = new Pen(Brushes.Red, 3)
            {
                DashStyle = DashStyles.Dash,
                DashCap = PenLineCap.Flat
            };

            //Create Fake Rectangle 
            _fakeRect = new Rectangle { Fill = Brushes.Transparent, Width = original.Width, Height = original.Height };
            
        }
        public (double X, double Y) Adge 
        { 
            get { return (_fakeRect.Height / 2, _fakeRect.Width / 2); } 
        }
        public Point StartPoint { get; set; }
        public bool IsDragStarted { get; set; }
        public bool IsDragRestricted { get; set; }
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
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));

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

            Rect rect = Source.Bounds;
            rect.Offset(Width / 2, Height / 2);
            ((RouteItem)Source).DrawElement(dc, _brush, _pen);
        }
        private void UpdatePosition()
        {
            var adornerLayer = Parent as AdornerLayer;
            adornerLayer?.Update(AdornedElement);
        }
    }
}

    

