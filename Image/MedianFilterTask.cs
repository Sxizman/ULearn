using System;
using System.Linq;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		/* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
		public static double[,] MedianFilter(double[,] original)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);

			var filtered = new double[width, height];
			for (var x = 0; x < width; ++x)
				for (var y = 0; y < height; ++y)
					filtered[x, y] = MedianFilter(original, x, y);
			return filtered;
		}

		public static double MedianFilter(double[,] original, int x, int y)
        {
			var pixels = GetClipPixels(original, x, y, 1);
			Array.Sort(pixels);
			return (pixels[pixels.Length / 2] + pixels[(pixels.Length - 1) / 2]) / 2;
		}

		public static double[] GetClipPixels(double[,] original, int xCenter, int yCenter, int radius)
        {
			var xMin = Math.Max(xCenter - radius, 0);
			var yMin = Math.Max(yCenter - radius, 0);
			var xMax = Math.Min(xCenter + radius + 1, original.GetLength(0));
			var yMax = Math.Min(yCenter + radius + 1, original.GetLength(1));
			
			return MakeOneDimensionalArray(original, xMin, yMin, xMax, yMax);
        }

		public static double[] MakeOneDimensionalArray(double[,] original, int xMin, int yMin, int xMax, int yMax)
		{
			var width = xMax - xMin;
			var height = yMax - yMin;

			var pixels = new double[width * height];
			for (var x = xMin; x < xMax; ++x)
				for (var y = yMin; y < yMax; ++y)
					pixels[(x - xMin) + (y - yMin) * width] = original[x, y];
			return pixels;
		}
	}
}