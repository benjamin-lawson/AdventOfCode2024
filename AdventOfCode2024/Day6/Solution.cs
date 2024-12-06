using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day6
{
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            int movedSpaces = SimulateGuardMovement(GenerateMap(inputData), out var hitTurnLimit);
            Console.WriteLine(movedSpaces);
        }

        private char[][] GenerateMap(string[] inputData)
        {
            char[][] map = new char[inputData.Length][];
            for (int i = 0; i < inputData.Length; i++)
            {
                map[i] = inputData[i].ToCharArray();
            }
            return map;
        }

        public Tuple<int, int> GetStartingGuardPosition(char[][] inputData)
        {
            for (int i = 0; i < inputData.Length; i++)
            {
                for (int j = 0; j < inputData[i].Length; j++)
                {
                    if (inputData[i][j] == '^') return Tuple.Create(j, i);
                }
            }
            return Tuple.Create(0, 0);
        }

        public Tuple<int, int> GetMovementOffset(char character)
        {
            if (character == '^') return Tuple.Create(0, -1);
            else if (character == 'v') return Tuple.Create(0, 1);
            else if (character == '>') return Tuple.Create(1, 0);
            else if (character == '<') return Tuple.Create(-1, 0);
            else return Tuple.Create(0, 0);
        }

        public char RotateCharacter(char character)
        {
            if (character == '^') return '>';
            else if (character == '>') return 'v';
            else if (character == 'v') return '<';
            else if (character == '<') return '^';
            else return '^';
        }

        public bool IsInBounds(Tuple<int, int> position, char[][] map)
        {
            return position.Item1 >= 0
                && position.Item2 >= 0
                && position.Item1 < map[0].Length
                && position.Item2 < map.Length;
        }

        public void PrintMap(char[][] map, Tuple<int, int> position, char character)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (x == position.Item1 && y == position.Item2) sb.Append(character);
                    else sb.Append(map[y][x]);
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }

        public int SimulateGuardMovement(char[][] map, out bool hitTurnLimit, bool printMap = false)
        {
            int moveLimit = map.Length * map[0].Length;
            HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
            var position = GetStartingGuardPosition(map);
            char character = '^';
            visited.Add(position);
            int moveNumber = 0;

            while (IsInBounds(position, map) && moveNumber <= moveLimit)
            {
                moveNumber++;
                var movementOffset = GetMovementOffset(character);
                var newPos = position.Add(movementOffset);

                if (!IsInBounds(newPos, map)) break;

                if (map[newPos.Item2][newPos.Item1] == '#')
                {
                    character = RotateCharacter(character);
                    continue;
                }

                if (!visited.Contains(newPos)) visited.Add(newPos);
                position = newPos;
                if (printMap) PrintMap(map, position, character);
            }

            hitTurnLimit = moveNumber >= moveLimit;
            return visited.Count;
        }

        public void SolvePart2(string[] inputData)
        {

            char[][] originalMap = GenerateMap(inputData);
            int totalObstaclePlacements = 0;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int y = 0; y < inputData.Length; y++)
            {
                for (int x = 0; x < inputData[y].Length; x++)
                {
                    char[][] newMap = originalMap.Select(l => l.Select(c => c).ToArray()).ToArray();
                    newMap[y][x] = '#';
                    SimulateGuardMovement(newMap, out var hitTurnLimit);
                    if (hitTurnLimit) totalObstaclePlacements++;
                }
            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            Console.WriteLine($"Total Valid Obstacle Placements: {totalObstaclePlacements}");
        }
    }

    static class TupleExtension
    {
        public static Tuple<int, int> Add(this Tuple<int, int> t1, Tuple<int, int> t2) => Tuple.Create(t1.Item1 + t2.Item1, t1.Item2 + t2.Item2);
    }
}
