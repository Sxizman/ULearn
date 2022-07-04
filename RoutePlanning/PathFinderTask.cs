using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			var distanceTable = MakeDistanceTable(checkpoints);

			var currentRoute = new Route
			{
				CheckpointsOrder = new List<int> { 0 },
				Length = 0
			};
			var shortestRoute = new Route
			{
				CheckpointsOrder = new List<int>(),
				Length = double.PositiveInfinity
			};

			FindBestCheckpointsOrder(distanceTable, currentRoute, shortestRoute);
			return shortestRoute.CheckpointsOrder.ToArray();
		}

		private static void FindBestCheckpointsOrder(double[,] distanceTable, Route currentRoute, Route shortestRoute)
        {
			if (currentRoute.Length >= shortestRoute.Length)
				return;

			var checkpointsCount = distanceTable.GetLength(0);
			if (currentRoute.CheckpointsOrder.Count == checkpointsCount)
            {
				currentRoute.Copy(shortestRoute);
				return;
            }

			for (var i = 0; i < checkpointsCount; ++i)
			{
				if (currentRoute.CheckpointsOrder.IndexOf(i) != -1)
					continue;

				currentRoute.AddCheckpoint(distanceTable, i);
				FindBestCheckpointsOrder(distanceTable, currentRoute, shortestRoute);
				currentRoute.RemoveLastCheckpoint(distanceTable);
			}
        }

		private static double[,] MakeDistanceTable(Point[] checkpoints)
        {
			var size = checkpoints.Length;
			var distanceTable = new double[size, size];
			for (var i = 0; i < size; ++i)
				for (var j = 0; j < size; ++j)
					distanceTable[i, j] = GetDistanceBetween(checkpoints[i], checkpoints[j]);
			return distanceTable;
		}

		private static double GetDistanceBetween(Point p1, Point p2)
        {
			var dx = (double)(p1.X - p2.X);
			var dy = (double)(p1.Y - p2.Y);
			return Math.Sqrt(dx * dx + dy * dy);
        }

		private class Route
        {
			public List<int> CheckpointsOrder { get; set; }
			public double Length { get; set; }

			public void AddCheckpoint(double[,] distanceTable, int index)
            {
				var lastIndex = CheckpointsOrder.Count - 1;
				Length += distanceTable[CheckpointsOrder[lastIndex], index];
				CheckpointsOrder.Add(index);
			}

			public void RemoveLastCheckpoint(double[,] distanceTable)
            {
				var lastIndex = CheckpointsOrder.Count - 1;
				Length -= distanceTable[CheckpointsOrder[lastIndex], CheckpointsOrder[lastIndex - 1]];
				CheckpointsOrder.RemoveAt(lastIndex);
            }

			public void Copy(Route destination)
            {
				destination.CheckpointsOrder = new List<int>(CheckpointsOrder);
				destination.Length = Length;
            }
        }
	}
}