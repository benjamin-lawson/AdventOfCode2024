using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day3
{
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            int total = 0;
            foreach (var line in inputData)
            {
                var matches = Regex.Matches(line, @"mul\((?<num1>\d+),(?<num2>\d+)\)");
                foreach (Match match in matches)
                {
                    total += ParseMul(match);
                }
            }

            Console.WriteLine($"Total Value: {total}");
        }

        public int ParseMul(Match instruct)
        {
            return int.Parse(instruct.Groups["num1"].Value) * int.Parse(instruct.Groups["num2"].Value);
        }

        public void SolvePart2(string[] inputData)
        {
            int total = 0;
            bool addActivated = true;
            foreach (var line in inputData)
            {
                var mulMatches = Regex.Matches(line, @"(?<cmd>mul)\((?<num1>\d+),(?<num2>\d+)\)");
                var doMatches = Regex.Matches(line, @"(?<cmd>do)\(\)");
                var dontMatches = Regex.Matches(line, @"(?<cmd>don't)\(\)");

                var matches = mulMatches
                    .Union(doMatches)
                    .Union(dontMatches)
                    .OrderBy(m => m.Index);

                foreach (Match match in matches)
                {
                    if (match.Groups["cmd"].Value == "mul" && addActivated)
                    {
                        total += ParseMul(match);
                    }
                    else if (match.Groups["cmd"].Value == "do")
                    {
                        addActivated = true;
                    }
                    else if (match.Groups["cmd"].Value == "don't")
                    {
                        addActivated = false;
                    }
                }
            }

            Console.WriteLine($"Total Value: {total}");
        }
    }
}
