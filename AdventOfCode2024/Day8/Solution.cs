using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day8
{
    struct Point
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
    }

    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            int height = inputData.Length;
            int width = inputData[0].Length;

            List<Point> allAntinodes = new List<Point>();

            var nodeMap = BuildNodeMap(inputData);
            foreach (var node in nodeMap)
            {
                allAntinodes.AddRange(GetAntinodes(node.Value, width, height));
            }

            Console.WriteLine($"Total Antinodes: {allAntinodes.Distinct().Count()}");
        }

        private Dictionary<char, List<Point>> BuildNodeMap(string[] data)
        {
            int height = data.Length;
            int width = data[0].Length;

            Dictionary<char, List<Point>> nodes = new Dictionary<char, List<Point>>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char currChar = data[y][x];
                    if (currChar != '.' && currChar != '\n')
                    {
                        if (!nodes.ContainsKey(currChar)) nodes.Add(currChar, new List<Point>());

                        nodes[currChar].Add(
                            new Point(x, y)
                        );
                    }
                }
            }

            return nodes;
        }

        private List<Point> GetAntinodes(List<Point> nodes, int width, int height)
        {
            List<Point> antinodes = new List<Point>();
            foreach (Point p1 in nodes)
            {
                foreach (Point p2 in nodes)
                {
                    if (p1.Equals(p2)) continue;

                    int dx = p2.X - p1.X;
                    int dy = p2.Y - p1.Y;

                    Point a1 = new Point(p1.X - dx, p1.Y - dy);
                    if (IsValidPoint(a1, width, height)) antinodes.Add(a1);

                    Point a2 = new Point(p2.X + dx, p2.Y + dy);
                    if (IsValidPoint(a2, width, height)) antinodes.Add(a2);
                }
            }

            return antinodes;
        }

        private List<Point> GetAntinodesExtended(List<Point> nodes, int width, int height)
        {
            List<Point> antinodes = new List<Point>();
            foreach (Point p1 in nodes)
            {
                foreach (Point p2 in nodes)
                {
                    if (p1.Equals(p2)) continue;

                    int dx = p2.X - p1.X;
                    int dy = p2.Y - p1.Y;

                    Point p3 = new Point(p1.X, p1.Y);
                    while (IsValidPoint(p3, width, height))
                    {
                        if (!antinodes.Contains(p3)) antinodes.Add(p3);

                        p3 = new Point(p3.X + dx, p3.Y + dy);
                    }

                    p3 = new Point(p2.X, p2.Y);
                    while (IsValidPoint(p3, width, height))
                    {
                        if (!antinodes.Contains(p3)) antinodes.Add(p3);

                        p3 = new Point(p3.X - dx, p3.Y - dy);
                    }
                }
            }

            return antinodes;
        }

        private static bool IsValidPoint(Point p, int width, int height)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height;
        }

        public void SolvePart2(string[] inputData)
        {
            int height = inputData.Length;
            int width = inputData[0].Length;

            List<Point> allAntinodes = new List<Point>();

            var nodeMap = BuildNodeMap(inputData);
            foreach (var node in nodeMap)
            {
                allAntinodes.AddRange(GetAntinodesExtended(node.Value, width, height));
            }

            Console.WriteLine($"Total Antinodes: {allAntinodes.Distinct().Count()}");
        }
    }
}
