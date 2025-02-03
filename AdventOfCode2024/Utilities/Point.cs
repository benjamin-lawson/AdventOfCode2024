using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Utilities
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Point)) return false;

            Point v = (Point)obj;
            return X == v.X && Y == v.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public static bool IsPointInBounds(Point p1, int minX, int minY, int maxX, int maxY) => 
            IsPointInBounds(p1.X, p1.Y, minX, minY, maxX, maxY);

        public static bool IsPointInBounds(int x, int y, int minX, int minY, int maxX, int maxY)
        {
            return x >= minX && x < maxX && y >= minY && y < maxY;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}
