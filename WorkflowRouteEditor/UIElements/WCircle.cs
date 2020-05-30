using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WorkflowRouteEditor.Control;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.WUIElements
{

	internal class WCircle : WUIElement<Route>
	{
		public WCircle(VisualHostContainer parent, Route route) :
			base(parent, route)
		{
			Radius = 50;
		}
		public Point Center { get; set; }
		public double Radius { get; set; }

		protected override void OnRender(DrawingContext dc)
		{
			dc.DrawEllipse(Brushes.Red, (Pen)null, Center, Radius, Radius);
			
			var text = new FormattedText(
					Item.Name,
					CultureInfo.GetCultureInfo("ru-RU"),
					FlowDirection.LeftToRight,
					new Typeface("Verdana"),
					18,
					Brushes.Black,
					1.0);

			dc.DrawText(text, new Point(Center.X - text.WidthIncludingTrailingWhitespace / 2.0, Center.Y/2 + text.Height / 2.0));
		}
	}
}
