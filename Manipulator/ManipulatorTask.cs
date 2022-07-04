using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var wristPosition = GetJointPosition(new PointD(x, y), Manipulator.Palm, Math.PI - alpha);
            var wristDistanceFromCenter = GetDistanceBetween(new PointD(0, 0), wristPosition);

            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, wristDistanceFromCenter);
            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, wristDistanceFromCenter, Manipulator.Forearm) +
                Math.Atan2(wristPosition.Y, wristPosition.X);
            var wrist = -(alpha + shoulder + elbow);

            return (double.IsNaN(elbow) || double.IsNaN(shoulder)) ?
                new double[] { double.NaN, double.NaN, double.NaN } :
                new double[] { shoulder, elbow, wrist };
        }

        public static PointD GetJointPosition(PointD origin, double length, double absoluteAngle)
        {
            return new PointD(
                origin.X + length * Math.Cos(absoluteAngle),
                origin.Y + length * Math.Sin(absoluteAngle));
        }

        public static double GetDistanceBetween(PointD p1, PointD p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            var maxDeviation = Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm;

            var random = new Random();
            for (var i = 0; i < 100; ++i)
            {
                var alpha = random.NextDouble() * Math.PI * 2;
                var x = 0.0;
                var y = 0.0;
                do
                {
                    x = random.NextDouble() * maxDeviation * 2 - maxDeviation;
                    y = random.NextDouble() * maxDeviation * 2 - maxDeviation;
                } while (IsInBoundaryArea(new PointD(x, y), alpha));

                TestMoveManipulatorTo_Case(x, y, alpha);
            }
        }

        public void TestMoveManipulatorTo_Case(double x, double y, double alpha)
        {
            var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
            if (IsInReachableArea(new PointD(x, y), alpha))
            {
                var palmEnd = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2])[2];
                Assert.AreEqual(x, palmEnd.X, 1e-4);
                Assert.AreEqual(y, palmEnd.Y, 1e-4);
            }
            else
            {
                CollectionAssert.AreEqual(new double[] { double.NaN, double.NaN, double.NaN }, angles);
            }
        }

        public bool IsInReachableArea(PointD point, double alpha)
        {
            var rMin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
            var rMax = Manipulator.UpperArm + Manipulator.Forearm;
            var r = GetDistanceFromReachableAreaCenter(point, alpha);
            return r >= rMin && r <= rMax;
        }

        public bool IsInBoundaryArea(PointD point, double alpha)
        {
            var rMin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
            var rMax = Manipulator.UpperArm + Manipulator.Forearm;
            var r = GetDistanceFromReachableAreaCenter(point, alpha);
            return Math.Abs(r - rMin) < 1e-5 || Math.Abs(r - rMax) < 1e-5;
        }

        public double GetDistanceFromReachableAreaCenter(PointD point, double alpha)
        {
            var areaCenter = ManipulatorTask.GetJointPosition(new PointD(0, 0), Manipulator.Palm, -alpha);
            return ManipulatorTask.GetDistanceBetween(point, areaCenter);
        }
    }
}