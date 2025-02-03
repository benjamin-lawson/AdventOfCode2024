using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode2024.Utilities;

namespace AdventOfCode2024.Day14
{
    public class Robot
    {
        public Point CurrentPosition;
        public Point Movement;

        public static int MaxX = -1;
        public static int MaxY = -1;

        public void Move()
        {
            Point newPos = CurrentPosition + Movement;

            if (newPos.X < 0) newPos.X += MaxX;
            else if (newPos.X >= MaxX) newPos.X -= MaxX;

            if (newPos.Y < 0) newPos.Y += MaxY;
            else if (newPos.Y >= MaxY) newPos.Y -= MaxY;

            CurrentPosition = newPos;
        }

        public int GetQuadrantNumber()
        {
            int vertQuad = MaxX / 2;
            int horQuad = MaxY / 2;

            if (CurrentPosition.X == vertQuad || CurrentPosition.Y == horQuad) return -1;
            else if (CurrentPosition.X < vertQuad && CurrentPosition.Y < horQuad) return 0;
            else if (CurrentPosition.X > vertQuad && CurrentPosition.Y < horQuad) return 1;
            else if (CurrentPosition.X < vertQuad && CurrentPosition.Y > horQuad) return 2;
            else if (CurrentPosition.X > vertQuad && CurrentPosition.Y > horQuad) return 3;

            return -1;
        }
    }

    public class Solution : ISolution
    {
        private Regex _headerRegex = new Regex(@"x=([\d-]+) y=([\d-]+)");
        private Regex _inputRegex = new Regex(@"p=([\d-]+),([\d-]+) v=([\d-]+),([\d-]+)");

        public void SolvePart1(string[] inputData)
        {
            int simulationCount = 100;

            var robots = ParseInput(inputData);

            for (int i = 0; i < simulationCount; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }
            }

            int[] quadCounts = new int[4];
            foreach (var robot in robots)
            {
                int quadrant = robot.GetQuadrantNumber();
                if (quadrant == -1) continue;
                quadCounts[quadrant]++;
            }

            int total = 1;
            foreach (int quadCount in quadCounts)
            {
                total *= quadCount;
            }

            Console.WriteLine($"Total: {total}");
        }

        public List<Robot> ParseInput(string[] inputData)
        {
            List<Robot> robots = new List<Robot>();
            bool isFirstLine = true;

            foreach (var line in inputData)
            {
                if (isFirstLine)
                {
                    var headerMatch = _headerRegex.Match(line);
                    Robot.MaxX = int.Parse(headerMatch.Groups[1].Value);
                    Robot.MaxY = int.Parse(headerMatch.Groups[2].Value);
                    isFirstLine = false;
                    continue;
                }

                var match = _inputRegex.Match(line);

                robots.Add(new Robot
                {
                    CurrentPosition = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    Movement = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });
            }

            return robots;
        }

        private void PrintBoard(List<Robot> robots)
        {
            for (int y = 0; y < Robot.MaxY; y++)
            {
                for (int x = 0; x < Robot.MaxX; x++)
                {
                    int robotCount = robots.Count(r => r.CurrentPosition.Equals(new Point(x, y)));
                    Console.Write(robotCount == 0 ? "." : robotCount);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void SolvePart2(string[] inputData)
        {
            int simulationCount = int.MaxValue;

            var robots = ParseInput(inputData);

            for (int i = 0; i < simulationCount; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }

                if (robots.Select(r => r.CurrentPosition).Distinct().Count() == robots.Count())
                {
                    Console.WriteLine($"After {i + 1} second(s)");
                    break;
                }
            }
        }
    }
}
