using System;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var threshold = FindThreshold(original, whitePixelsFraction);

			var filtered = new double[width, height];
			for (var x = 0; x < width; ++x)
				for (var y = 0; y < height; ++y)
					filtered[x, y] = ThresholdFilter(original[x, y], threshold);
			return filtered;
		}

		public static double ThresholdFilter(double pixel, double threshold)
		{
			return pixel < threshold ? 0 : 1;
		}

		public static double FindThreshold(double[,] original, double whitePixelsFraction)
        {
			var pixels = MakeOneDimensionalArray(original);
			Array.Sort(pixels);

			var minWhitePixels = (int)(whitePixelsFraction * pixels.Length);
			return (minWhitePixels > 0) ?
				pixels[pixels.Length - minWhitePixels] :
				double.PositiveInfinity;
        }

		public static double[] MakeOneDimensionalArray(double[,] original)
        {
			var width = original.GetLength(0);
			var height = original.GetLength(1);

			var pixels = new double[width * height];
			for (var x = 0; x < width; ++x)
				for (var y = 0; y < height; ++y)
					pixels[x + y * width] = original[x, y];
			return pixels;
		}
	}
}