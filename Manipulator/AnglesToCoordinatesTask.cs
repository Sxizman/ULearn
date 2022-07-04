using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPosition = GetJointPosition(new PointD(0, 0), Manipulator.UpperArm, shoulder);
            var elbowAbsoluteAngle = shoulder + elbow - Math.PI;
            var wristPosition = GetJointPosition(elbowPosition, Manipulator.Forearm, elbowAbsoluteAngle);
            var wristAbsoluteAngle = elbowAbsoluteAngle + wrist - Math.PI;
            var palmEndPosition = GetJointPosition(wristPosition, Manipulator.Palm, wristAbsoluteAngle);

            return new PointF[]
            {
                new PointF((float)elbowPosition.X, (float)elbowPosition.Y),
                new PointF((float)wristPosition.X, (float)wristPosition.Y),
                new PointF((float)palmEndPosition.X, (float)palmEndPosition.Y)
            };
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

    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI,
            Manipulator.Forearm + Manipulator.Palm,
            Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI * 3 / 2,
            Manipulator.Forearm,
            Manipulator.UpperArm + Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI * 3 / 2, Math.PI * 3 / 2,
            -Manipulator.Forearm,
            Manipulator.UpperArm - Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI,
            0,
            Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
        [TestCase(0, Math.PI, Math.PI,
            Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm,
            0)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");

            Assert.AreEqual(Manipulator.UpperArm,
                AnglesToCoordinatesTask.GetDistanceBetween(
                    new PointD(0, 0),
                    new PointD(joints[0].X, joints[0].Y)), 1e-5);
            Assert.AreEqual(Manipulator.Forearm,
                AnglesToCoordinatesTask.GetDistanceBetween(
                    new PointD(joints[0].X, joints[0].Y),
                    new PointD(joints[1].X, joints[1].Y)), 1e-5);
            Assert.AreEqual(Manipulator.Palm,
                AnglesToCoordinatesTask.GetDistanceBetween(
                    new PointD(joints[1].X, joints[1].Y),
                    new PointD(joints[2].X, joints[2].Y)), 1e-5);
        }
    }
}