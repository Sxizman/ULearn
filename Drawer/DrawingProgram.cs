using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Drawer
    {
        private static float _x, _y;
        private static Graphics _canvas;

        public static void Initialize(Graphics canvas)
        {
            _canvas = canvas;
            _canvas.SmoothingMode = SmoothingMode.None;
            _canvas.Clear(Color.Black);
        }

        public static void SetPosition(double x, double y)
        {
            _x = (float)x;
            _y = (float)y;
        }

        public static void DrawLine(Pen pen, double distance, double angle)
        {
            var xOld = _x;
            var yOld = _y;
            Move(distance, angle);
            _canvas.DrawLine(pen, xOld, yOld, _x, _y);
        }

        public static void Move(double distance, double angle)
        {
            _x = (float)(_x + distance * Math.Cos(angle));
            _y = (float)(_y + distance * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double rotationAngle, Graphics canvas)
        {
            Drawer.Initialize(canvas);

            var size = Math.Min(width, height);
            var edgeLength = size * 0.375;
            var edgeThickness = size * 0.04;

            var x0 = width / 2 - (edgeLength + edgeThickness) / 2;
            var y0 = height / 2 - (edgeLength + edgeThickness) / 2;
            Drawer.SetPosition(x0, y0);

            for (int i = 0; i < 4; ++i)
            {
                double angle = -Math.PI * (i / 2.0);
                Drawer.DrawLine(Pens.Yellow, edgeLength, angle);
                Drawer.DrawLine(Pens.Yellow, edgeThickness * Math.Sqrt(2), angle + Math.PI / 4);
                Drawer.DrawLine(Pens.Yellow, edgeLength, angle + Math.PI);
                Drawer.DrawLine(Pens.Yellow, edgeLength - edgeThickness, angle + Math.PI / 2);

                Drawer.Move(edgeThickness, angle + Math.PI);
                Drawer.Move(edgeThickness * Math.Sqrt(2), angle + Math.PI * (3 / 4.0));
            }
        }
    }
}