using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		public const double GravityAcceleration = 9.8;

		/// <param name="v">Начальная скорость</param>
		/// <param name="distance">Расстояние до цели</param>
		/// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>
		public static double FindSightAngle(double v, double distance)
		{
			var doubleAngleSin = (distance * GravityAcceleration) / (v * v);
			if (doubleAngleSin < 0 || doubleAngleSin > 1)
				return double.NaN;

			return Math.Asin(doubleAngleSin) / 2;
		}
	}
}