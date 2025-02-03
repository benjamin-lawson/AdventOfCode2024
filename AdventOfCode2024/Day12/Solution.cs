using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.Utilities;

namespace AdventOfCode2024.Day12
{
    public class GardenPlot
    {
        public char Id;
        public List<Point> Points = new List<Point>();

        public int Area => Points.Count;

        public int GetPerimeter(char[][] gardenMap)
        {
            int height = gardenMap.Length;
            int width = gardenMap[0].Length;
            int perimeter = 0;
            foreach (var point in Points)
            {
                if (!Point.IsPointInBounds(point.X + 1, point.Y, 0, 0, width, height) || gardenMap[point.Y][point.X + 1] != Id) perimeter++;
                if (!Point.IsPointInBounds(point.X - 1, point.Y, 0, 0, width, height) || gardenMap[point.Y][point.X - 1] != Id) perimeter++;
                if (!Point.IsPointInBounds(point.X, point.Y + 1, 0, 0, width, height) || gardenMap[point.Y + 1][point.X] != Id) perimeter++;
                if (!Point.IsPointInBounds(point.X, point.Y - 1, 0, 0, width, height) || gardenMap[point.Y - 1][point.X] != Id) perimeter++;
            }
            return perimeter;
        }

        public int GetSides(char[][] gardenMap)
        {
            int height = gardenMap.Length;
            int width = gardenMap[0].Length;
            int sides = 0;

            Tuple<int, int>[] offsets =
            [
                Tuple.Create(1,1),
                Tuple.Create(-1,1),
                Tuple.Create(1,-1),
                Tuple.Create(-1,-1)
            ];

            foreach (var point in Points)
            {
                foreach (var offset in offsets)
                {
                    char rowNeighbor, colNeighbor, diagNeighbor;

                    if (Point.IsPointInBounds(point.X + offset.Item1, point.Y, 0, 0, width, height))
                    {
                        rowNeighbor = gardenMap[point.Y][point.X + offset.Item1];
                    }
                    else
                    {
                        rowNeighbor = '*';
                    }

                    if (Point.IsPointInBounds(point.X, point.Y + offset.Item2, 0, 0, width, height))
                    {
                        colNeighbor = gardenMap[point.Y + offset.Item2][point.X];
                    }
                    else
                    {
                        colNeighbor = '*';
                    }

                    if (Point.IsPointInBounds(point.X + offset.Item1, point.Y + offset.Item2, 0, 0, width, height))
                    {
                        diagNeighbor = gardenMap[point.Y + offset.Item2][point.X + offset.Item1];
                    }
                    else
                    {
                        diagNeighbor = '*';
                    }

                    if (rowNeighbor != Id && colNeighbor != Id) sides++;
                    if (rowNeighbor == Id && colNeighbor == Id && diagNeighbor != Id) sides++;
                }
            }

            return sides;
        }

        public int GetPrice(char[][] gardenMap)
        {
            return Area * GetPerimeter(gardenMap);
        }

        public int GetBulkPrice(char[][] gardenMap) => Area * GetSides(gardenMap);
    }

    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            char[][] gardenMap = new char[inputData.Length][];
            bool[][] visitedMap = new bool[inputData.Length][];
            for (int i = 0; i < inputData.Length; i++)
            {
                gardenMap[i] = inputData[i].ToCharArray();
                visitedMap[i] = new bool[inputData[i].Length];
                for (int j = 0; j < inputData[i].Length; j++)
                {
                    visitedMap[i][j] = false;
                }
            }

            List<GardenPlot> plots = new List<GardenPlot>();
            for (int y = 0; y < gardenMap.Length; y++) 
            {
                for (int x = 0; x < gardenMap[y].Length; x++)
                {
                    if (!visitedMap[y][x])
                    {
                        plots.Add(TraverseGardenPlot(new Point(x, y), gardenMap, visitedMap));
                    }
                }
            }

            int totalPrice = plots.Sum(p => p.GetPrice(gardenMap));
            Console.WriteLine($"Total Price: {totalPrice}");
        }

        public GardenPlot TraverseGardenPlot(Point startPoint, char[][] gardenMap, bool[][] visitedMap)
        {
            GardenPlot plot = new GardenPlot();
            plot.Id = gardenMap[startPoint.Y][startPoint.X];

            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(startPoint);

            while (pointQueue.Count > 0)
            {
                Point point = pointQueue.Dequeue();
                visitedMap[point.Y][point.X] = true;
                plot.Points.Add(point);

                List<Point> potentialMoves = new List<Point>()
                {
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                };

                potentialMoves = potentialMoves.Where(p =>
                    Point.IsPointInBounds(p, 0, 0, gardenMap[0].Length, gardenMap.Length) &&
                    gardenMap[p.Y][p.X] == plot.Id &&
                    !visitedMap[p.Y][p.X] &&
                    !pointQueue.Contains(p)
                ).ToList();

                foreach (Point p in potentialMoves)
                {
                    pointQueue.Enqueue(p);
                }
            }

            return plot;
        } 

        public void SolvePart2(string[] inputData)
        {
            char[][] gardenMap = new char[inputData.Length][];
            bool[][] visitedMap = new bool[inputData.Length][];
            for (int i = 0; i < inputData.Length; i++)
            {
                gardenMap[i] = inputData[i].ToCharArray();
                visitedMap[i] = new bool[inputData[i].Length];
                for (int j = 0; j < inputData[i].Length; j++)
                {
                    visitedMap[i][j] = false;
                }
            }

            List<GardenPlot> plots = new List<GardenPlot>();
            for (int y = 0; y < gardenMap.Length; y++)
            {
                for (int x = 0; x < gardenMap[y].Length; x++)
                {
                    if (!visitedMap[y][x])
                    {
                        plots.Add(TraverseGardenPlot(new Point(x, y), gardenMap, visitedMap));
                    }
                }
            }

            int totalPrice = plots.Sum(p => p.GetBulkPrice(gardenMap));
            Console.WriteLine($"Total Price: {totalPrice}");
        }
    }
}
