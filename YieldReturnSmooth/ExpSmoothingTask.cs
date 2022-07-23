using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var smoothedY = double.NaN;
			foreach (var point in data)
            {
				if (double.IsNaN(smoothedY))
					smoothedY = point.OriginalY;
				else
					smoothedY = point.OriginalY * alpha + smoothedY * (1 - alpha);
				yield return point.WithExpSmoothedY(smoothedY);
            }
		}
	}
}