using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			return r1.Right >= r2.Left && r1.Left <= r2.Right && r1.Bottom >= r2.Top && r1.Top <= r2.Bottom;
		}

		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			var intersectionWidth = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left);
			if (intersectionWidth < 0)
				return 0;
			var intersectionHeight = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top);
			if (intersectionHeight < 0)
				return 0;
			return intersectionWidth * intersectionHeight;
		}

		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			if (r1.Left >= r2.Left && r1.Right <= r2.Right && r1.Top >= r2.Top && r1.Bottom <= r2.Bottom)
				return 0;
			if (r1.Left <= r2.Left && r1.Right >= r2.Right && r1.Top <= r2.Top && r1.Bottom >= r2.Bottom)
				return 1;
			return -1;
		}
	}
}