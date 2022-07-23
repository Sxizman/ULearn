using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var window = new Queue<double>();
			var windowSum = 0.0;
			foreach (var point in data)
            {
				window.Enqueue(point.OriginalY);
				windowSum += point.OriginalY;
				if (window.Count > windowWidth)
					windowSum -= window.Dequeue();
				yield return point.WithAvgSmoothedY(windowSum / window.Count);
            }
		}
	}
}