using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day4
{
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            var map = GenerateMap(inputData);
            int totalOccurrences = 0;
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'X')
                    {
                        totalOccurrences += CountHorizontalOccurrences(map, x, y);
                        totalOccurrences += CountVerticalOccurrences(map, x, y);
                        totalOccurrences += CountDiagonalOccurrences(map, x, y);
                    }
                }
            }

            Console.WriteLine($"Total Occurrences of \"XMAS\": {totalOccurrences}");
        }

        private char[][] GenerateMap(string[] inputData)
        {
            char[][] dataMap = new char[inputData.Length][];
            for (int i = 0; i < inputData.Length; i++)
            {
                dataMap[i] = inputData[i].ToCharArray();
            }
            return dataMap;
        }

        private int CountHorizontalOccurrences(char[][] map, int x, int y)
        {
            int total = 0;

            // Right
            string rightWord = "";
            for (int i = x; i < x + 4; i++)
            {
                if (i >= map[y].Length) break;

                rightWord += map[y][i];

                if (!"XMAS".StartsWith(rightWord)) break;
            }

            if (rightWord == "XMAS") total++;

            // Left
            string leftWord = "";
            for (int i = x; i > x - 4; i--)
            {
                if (i < 0) break;

                leftWord += map[y][i];

                if (!"XMAS".StartsWith(leftWord)) break;
            }

            if (leftWord == "XMAS") total++;

            return total;
        }

        private int CountVerticalOccurrences(char[][] map, int x, int y)
        {
            int total = 0;

            // Down
            string downWord= "";
            for (int i = y; i < y + 4; i++)
            {
                if (i >= map.Length) break;

                downWord += map[i][x];

                if (!"XMAS".StartsWith(downWord)) break;
            }

            if (downWord == "XMAS") total++;

            // Up
            string upWord = "";
            for (int i = y; i > y - 4; i--)
            {
                if (i < 0) break;

                upWord += map[i][x];

                if (!"XMAS".StartsWith(upWord)) break;
            }

            if (upWord == "XMAS") total++;

            return total;
        }

        private int CountDiagonalOccurrences(char[][] map, int x, int y)
        {
            int total = 0;
            string word = "";

            // Down Right
            for (int i = 0; i < 4; i++)
            {
                if (x + i >= map[0].Length || y + i >= map.Length) break;

                word += map[y + i][x + i];

                if (!"XMAS".StartsWith(word)) break;
            }

            if (word == "XMAS") total++;
            word = "";

            // Down Left
            for (int i = 0; i < 4; i++)
            {
                if (x - i < 0 || y + i >= map.Length) break;

                word += map[y + i][x - i];

                if (!"XMAS".StartsWith(word)) break;
            }

            if (word == "XMAS") total++;
            word = "";

            // Up Right
            for (int i = 0; i < 4; i++)
            {
                if (x + i >= map[0].Length || y - i < 0) break;

                word += map[y - i][x + i];

                if (!"XMAS".StartsWith(word)) break;
            }

            if (word == "XMAS") total++;
            word = "";

            // Up Left
            for (int i = 0; i < 4; i++)
            {
                if (x - i < 0 || y - i < 0) break;

                word += map[y - i][x - i];

                if (!"XMAS".StartsWith(word)) break;
            }

            if (word == "XMAS") total++;

            return total;
        }

        public void SolvePart2(string[] inputData)
        {
            var map = GenerateMap(inputData);
            int totalOccurrences = 0;
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'A')
                    {
                        totalOccurrences += IsValidCrossMas(map, x, y) ? 1 : 0;
                    }
                }
            }

            Console.WriteLine($"Total Occurrences of \"XMAS\": {totalOccurrences}");
        }

        private bool IsValidCrossMas(char[][] map, int x, int y)
        {
            if (x + 1 >= map[0].Length ||
                x - 1 < 0 ||
                y + 1 >= map.Length ||
                y - 1 < 0) return false;

            int diagCount = 0;
            if (map[y-1][x-1] == 'M' && map[y + 1][x+1] == 'S')
            {
                diagCount++;
            }
            if (map[y - 1][x - 1] == 'S' && map[y + 1][x + 1] == 'M')
            {
                diagCount++;
            }
            if (map[y + 1][x - 1] == 'M' && map[y - 1][x + 1] == 'S')
            {
                diagCount++;
            }
            if (map[y + 1][x - 1] == 'S' && map[y - 1][x + 1] == 'M')
            {
                diagCount++;
            }

            return diagCount == 2;
        }
    }
}
