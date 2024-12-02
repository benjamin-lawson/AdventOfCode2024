using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day1
{
    public class Solution : ISolution
    {
        private List<int> lst1;
        private List<int> lst2;

        private void ReadInputData(string[] inputData)
        {
            lst1 = new();
            lst2 = new();

            for (int i = 0; i < inputData.Length; i++)
            {
                var splitData = inputData[i].Split();
                lst1.Add(int.Parse(splitData.First()));
                lst2.Add(int.Parse(splitData.Last()));
            }
        }

        public void SolvePart1(string[] inputData)
        {
            ReadInputData(inputData);
            lst1.Sort();
            lst2.Sort();

            int totalDistance = 0;
            for (int i = 0; i < lst1.Count; i++)
            {
                totalDistance += GetDistance(lst1[i], lst2[i]);
            }
            Console.WriteLine($"Total Distance: {totalDistance}");
        }

        public void SolvePart2(string[] inputData)
        {
            ReadInputData(inputData);
            var rightListCounts = lst2
                .GroupBy(
                    (x) => x, 
                    (x) => x, 
                    (x, vals) => new { Key = x, Value = vals.Count() })
                .ToDictionary((x) => x.Key, (x) => x.Value);

            int similarityScore = 0;
            foreach (var num in lst1)
            {
                if (rightListCounts.ContainsKey(num))
                {
                    similarityScore += num * rightListCounts[num];
                }
            }

            Console.WriteLine($"Similarity Score: {similarityScore}");
        }

        private int GetDistance(int x, int y) => Math.Abs(x - y);
    }
}
