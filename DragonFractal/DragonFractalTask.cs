using System;
using System.Drawing;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
			var random = new Random(seed);

			var x = 1.0;
			var y = 0.0;
			for (var i = 0; i < iterationsCount; ++i)
            {
				if (random.Next() % 2 == 0)
					Transform(ref x, ref y, Math.PI / 4, Math.Sqrt(2) / 2, 0, 0);
				else
					Transform(ref x, ref y, Math.PI * (3 / 4.0), Math.Sqrt(2) / 2, 1, 0);

				pixels.SetPixel(x, y);
            }
		}

		public static void Transform(ref double x, ref double y, double rotationAngle, double scaleFactor, double xOffset, double yOffset)
        {
			var sin = Math.Sin(rotationAngle);
			var cos = Math.Cos(rotationAngle);

			var xNew = (x * cos - y * sin) * scaleFactor + xOffset;
			var yNew = (x * sin + y * cos) * scaleFactor + yOffset;
			x = xNew;
			y = yNew;
        }
	}
}