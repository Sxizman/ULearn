namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			for (var i = 0; !robot.Finished; ++i)
            {
				if (i % 2 == 0)
					MoveInDirection(robot, (i % 4 == 0) ? Direction.Right : Direction.Left, width - 3);
				else
					MoveInDirection(robot, Direction.Down, 2);
			}
		}

		public static void MoveInDirection(Robot robot, Direction direction, int steps)
		{
			for (var i = 0; i < steps; ++i)
				robot.MoveTo(direction);
		}
	}
}