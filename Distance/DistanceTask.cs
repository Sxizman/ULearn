using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
			var d_bx_ax = bx - ax;
			var d_by_ay = by - ay;

			var d_x_ax = x - ax;
			var d_y_ay = y - ay;
			if (d_x_ax * d_bx_ax + d_y_ay * d_by_ay <= 0)
				return Math.Sqrt(d_x_ax * d_x_ax + d_y_ay * d_y_ay);

			var d_x_bx = x - bx;
			var d_y_by = y - by;
			if (d_x_bx * d_bx_ax + d_y_by * d_by_ay >= 0)
				return Math.Sqrt(d_x_bx * d_x_bx + d_y_by * d_y_by);

			return Math.Abs(d_by_ay * d_x_ax - d_bx_ax * d_y_ay) / Math.Sqrt(d_bx_ax * d_bx_ax + d_by_ay * d_by_ay);
		}
	}
}