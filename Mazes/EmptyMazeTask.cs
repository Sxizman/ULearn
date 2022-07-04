namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			MoveInDirection(robot, Direction.Right, width - 3);
			MoveInDirection(robot, Direction.Down, height - 3);
		}

		public static void MoveInDirection(Robot robot, Direction direction, int steps)
        {
			for (var i = 0; i < steps; ++i)
				robot.MoveTo(direction);
        }
	}
}