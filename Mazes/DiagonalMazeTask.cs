using System;

namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			var longStep = (Math.Max(width, height) - 3) / (Math.Min(width, height) - 3);
			for (var i = 0; !robot.Finished; ++i)
            {
				if (i % 2 == 0)
					MoveInDirection(robot, width > height ? Direction.Right : Direction.Down, longStep);
				else
					MoveInDirection(robot, width > height ? Direction.Down : Direction.Right, 1);
			}				
		}

		public static void MoveInDirection(Robot robot, Direction direction, int steps)
		{
			for (var i = 0; i < steps; ++i)
				robot.MoveTo(direction);
		}
	}
}