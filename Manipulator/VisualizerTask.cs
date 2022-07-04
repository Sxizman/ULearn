using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public const double Delta = 0.05;

		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;

		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
			switch (key.KeyCode)
            {
				case Keys.Q:
					Shoulder += Delta;
					break;
				case Keys.A:
					Shoulder -= Delta;
					break;
				case Keys.W:
					Elbow += Delta;
					break;
				case Keys.S:
					Elbow -= Delta;
					break;
				default:
					return;
			}

			Wrist = -(Alpha + Shoulder + Elbow);

			form.Invalidate();
		}


		public static void MouseMove(Form form, MouseEventArgs e)
		{
			var updatedPosition = ConvertWindowToMath(new PointF(e.X, e.Y), GetShoulderPos(form));
			X = updatedPosition.X;
			Y = updatedPosition.Y;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
			Alpha += Delta * e.Delta;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void UpdateManipulator()
		{
			var angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
			if (double.IsNaN(angles[0]) || double.IsNaN(angles[1]) || double.IsNaN(angles[2]))
				return;

			Shoulder = angles[0];
			Elbow = angles[1];
			Wrist = angles[2];
		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);
			var jointsWithShoulder = new PointF[] { new PointF(), joints[0], joints[1], joints[2] };

			graphics.DrawString(
                $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}", 
                new Font(SystemFonts.DefaultFont.FontFamily, 12), 
                Brushes.DarkRed, 
                10, 
                10);
			DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

			for (var i = 0; i < 3; ++i)
				DrawArm(graphics, jointsWithShoulder[i], jointsWithShoulder[i + 1], shoulderPos);
			for (var i = 0; i < 4; ++i)
				DrawJoint(graphics, jointsWithShoulder[i], 5, shoulderPos);
		}

		private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush, 
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
		}

		private static void DrawJoint(Graphics graphics, PointF position, float radius, PointF shoulderPos)
        {
			var windowPosition = ConvertMathToWindow(position, shoulderPos);
			graphics.FillEllipse(JointBrush, windowPosition.X - radius, windowPosition.Y - radius, 2 * radius, 2 * radius);
		}

		private static void DrawArm(Graphics graphics, PointF firstJointPosition, PointF secondJointPosition, PointF shoulderPos)
        {
			var firstJointWindowPosition = ConvertMathToWindow(firstJointPosition, shoulderPos);
			var secondJointWindowPosition = ConvertMathToWindow(secondJointPosition, shoulderPos);
			graphics.DrawLine(ManipulatorPen, firstJointWindowPosition, secondJointWindowPosition);
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}
	}
}