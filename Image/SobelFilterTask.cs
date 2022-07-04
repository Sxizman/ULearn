using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var sxRadius = sx.GetLength(0) / 2;

            var filtered = new double[width, height];
            for (var x = sxRadius; x < width - sxRadius; ++x)
                for (var y = sxRadius; y < height - sxRadius; ++y)
                    filtered[x, y] = SobelFilter(g, x, y, sx);
            return filtered;
        }

        public static double SobelFilter(double[,] g, int xCenter, int yCenter, double[,] sx)
        {
            var sxRadius = sx.GetLength(0) / 2;

            var gx = 0.0;
            var gy = 0.0;
            for (var x = xCenter - sxRadius; x <= xCenter + sxRadius; ++x)
                for (var y = yCenter - sxRadius; y <= yCenter + sxRadius; ++y)
                {
                    gx += sx[x - (xCenter - sxRadius), y - (yCenter - sxRadius)] * g[x, y];
                    gy += sx[y - (yCenter - sxRadius), x - (xCenter - sxRadius)] * g[x, y];
                }
            return Math.Sqrt(gx * gx + gy * gy);
        }
    }
}