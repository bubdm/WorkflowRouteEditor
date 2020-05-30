using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace WorkflowRouteEditor.Control.Drawing
{
    internal static class GraphicsExtentions
    {
        static Point Empty = new Point(0, 0);

        public static Point CenterLocation(this Rect rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        public static bool IsPointEmpty(this Point pt)
        {
            return pt == Empty;
        }
        public static Point PointInLine(this Point from, Point to, double offset)
        {
            // tg of Angle in radians
            double d = Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));

            // Vector posions
            double X = to.X - from.X;
            double Y = to.Y - from.Y;

            // Point far from up to offset px
            return new Point(from.X + (X / d) * offset, from.Y + (Y / d) * offset);
        }
        public static bool LineHitTest(this Point from, Point to, Point test)
        {
            // Vector positions
            double X = to.X - from.X;
            double Y = to.Y - from.Y;

            var testX = Math.Round((test.X - from.X) / X, 1);
            var testY = Math.Round((test.Y - from.Y) / Y, 1);

            //if (Math.Abs(testX - testY) <= 0.1)
            //{
            //    Debug.WriteLine($"X:{X} Y:{Y} - TX:{testX} TY:{testY}");
            //}

            return Math.Abs(testX - testY) <= 0.1;
            //return testX == testY;
        }
        public static Geometry ArrowLineGeometry(this Point from, Point to)
        {
            // tg of Angle in radians
            double d = Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));

            // Vector positions
            double X = to.X - from.X;
            double Y = to.Y - from.Y;
            //Normal to line position
            double Xn = to.Y - from.Y;
            double Yn = from.X - to.X;

            // Center of the line
            double X0 = (from.X + to.X) / 2;
            double Y0 = (from.Y + to.Y) / 2;
            //Quater of the line
            // can be comment to draw the arrows at the line center
            X0 = (from.X + X0) / 2;
            Y0 = (from.Y + Y0) / 2;
            // Point far from center up to 10 px
            double X4 = X0 - (X / d) * 10;
            double Y4 = Y0 - (Y / d) * 10;

            // Arrow positions (length is 5 px long)
            double X5 = X4 + (Xn / d) * 5;
            double Y5 = Y4 + (Yn / d) * 5;
            double X6 = X4 - (Xn / d) * 5;
            double Y6 = Y4 - (Yn / d) * 5;

            return new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry(new Point(from.X, from.Y), new Point(to.X, to.Y)),
                    new LineGeometry(new Point(X0, Y0), new Point(X5, Y5)),
                    new LineGeometry(new Point(X0, Y0), new Point(X6, Y6))
                }
            };
        }
    }
}
