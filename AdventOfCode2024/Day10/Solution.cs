using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day10
{
    public struct Point
    {
        public readonly int X;
        public readonly int Y;

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
    }

    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            int[][] map = BuildMap(inputData);

            int totalPaths = 0;
            foreach (Point p in GetTrailheads(map))
            {
                int paths = TraverseTrail(map, p);
                totalPaths += paths;
            }

            Console.WriteLine($"Total Paths: {totalPaths}");
        }

        public int[][] BuildMap(string[] input)
        {
            int[][] map = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                map[i] = new int[input[i].Length];
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (input[i][j] == '.') map[i][j] = -1;
                    else map[i][j] = int.Parse(input[i][j].ToString());
                }
            }
            return map;
        }

        public List<Point> GetTrailheads(int[][] map)
        {
            List<Point> trailheads = new List<Point>();
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 0) trailheads.Add(new Point(j, i));
                }
            }
            return trailheads;
        }

        private static bool IsInboundsPoint(Point p, int width, int height)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height;
        }

        public int TraverseTrail(int[][] map, Point point, bool storeVisitedPoints = true, List<Point> visitedPoints = null)
        {
            if (visitedPoints == null) visitedPoints = new List<Point>();

            int trailHeight = map[point.Y][point.X];

            if (trailHeight == 9 && (!storeVisitedPoints || !visitedPoints.Contains(point)))
            {
                visitedPoints.Add(point);
                return 1;
            }

            List<Point> potentialMoves = new List<Point>()
            {
                new Point(point.X + 1, point.Y),
                new Point(point.X - 1, point.Y),
                new Point(point.X, point.Y + 1),
                new Point(point.X, point.Y - 1),
            };

            int height = map.Length;
            int width = map[0].Length;

            potentialMoves = potentialMoves.Where(p => IsInboundsPoint(p, width, height) && map[p.Y][p.X] == (trailHeight + 1)).ToList();

            if (potentialMoves.Count == 0) return 0;

            return potentialMoves.Sum(pm => TraverseTrail(map, pm, storeVisitedPoints, visitedPoints));
        }

        public void SolvePart2(string[] inputData)
        {
            int[][] map = BuildMap(inputData);

            int totalPaths = 0;
            foreach (Point p in GetTrailheads(map))
            {
                int paths = TraverseTrail(map, p, false);
                totalPaths += paths;
            }

            Console.WriteLine($"Total Paths: {totalPaths}");
        }
    }
}
