using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var window = new Queue<double>();
			var windowMaxes = new LinkedList<double>();
			foreach (var point in data)
            {
				window.Enqueue(point.OriginalY);
				while (windowMaxes.Count > 0 && point.OriginalY > windowMaxes.Last.Value)
					windowMaxes.RemoveLast();
				windowMaxes.AddLast(point.OriginalY);

				if (window.Count > windowWidth)
					if (window.Dequeue() == windowMaxes.First.Value)
						windowMaxes.RemoveFirst();

				yield return point.WithMaxY(windowMaxes.First.Value);
			}
		}
	}
}