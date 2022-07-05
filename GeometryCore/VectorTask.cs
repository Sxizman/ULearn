using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public Vector()
        {

        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector GetNormal()
        {
            return Geometry.GetNormal(this);
        }

        public Vector Add(Vector v)
        {
            return Geometry.Add(this, v);
        }

        public Vector Subtract(Vector v)
        {
            return Geometry.Subtract(this, v);
        }

        public double Dot(Vector v)
        {
            return Geometry.Dot(this, v);
        }

        public bool Belongs(Segment s)
        {
            return Geometry.IsVectorInSegment(this, s);
        }

        public double GetDistance(Segment s)
        {
            return Geometry.GetDistance(this, s);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public Segment()
        {
            Begin = new Vector();
            End = new Vector();
        }

        public Segment(Vector begin, Vector end)
        {
            Begin = new Vector(begin.X, begin.Y);
            End = new Vector(end.X, end.Y);
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector v)
        {
            return Geometry.IsVectorInSegment(v, this);
        }

        public double GetDistance(Vector v)
        {
            return Geometry.GetDistance(v, this);
        }
    }

    public static class Geometry
    {
        public static double GetLength(Vector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector GetNormal(Vector v)
        {
            return new Vector(v.Y, -v.X);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector Subtract(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static double Dot(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static double GetLength(Segment s)
        {
            return GetLength(Subtract(s.End, s.Begin));
        }

        public static bool IsVectorInSegment(Vector v, Segment s)
        {
            return GetDistance(v, s) < 1e-6;
        }

        public static double GetDistance(Vector v, Segment s)
        {
            Vector sv = Subtract(s.End, s.Begin);

            Vector v1 = Subtract(v, s.Begin);
            if (Dot(sv, v1) <= 0)
                return GetLength(v1);

            Vector v2 = Subtract(s.End, v);
            if (Dot(sv, v2) <= 0)
                return GetLength(v2);

            Vector svn = GetNormal(sv);
            return Math.Abs(Dot(svn, v1)) / GetLength(sv);
        }
    }
}