using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day7
{
    enum Operators
    {
        ADD,
        MUL,
        CON
    }

    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            long total = 0;
            foreach (string line in inputData)
            {
                long testValue = long.Parse(line.Split(':')[0].Trim());

                string equationSection = line.Split(":")[1].Trim();
                var equationValues = equationSection.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x.Trim())).ToArray();

                if (TestEquation(testValue, equationValues)) total += testValue;
            }

            Console.WriteLine($"Total Valid Equations: {total}");
        }

        public void SolvePart2(string[] inputData)
        {
            long total = 0;
            foreach (string line in inputData)
            {
                long testValue = long.Parse(line.Split(':')[0].Trim());

                string equationSection = line.Split(":")[1].Trim();
                var equationValues = equationSection.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x.Trim())).ToArray();

                if (TestEquation(testValue, equationValues, part2: true)) total += testValue;
            }

            Console.WriteLine($"Total Valid Equations: {total}");
        }

        private bool TestEquation(long target, long[] values, bool part2 = false)
        {
            if (values.Length == 1)
            {
                return target == values[0];
            }

            var subsequentTests = new List<bool>()
            {
                TestEquation(target, PerformOperation(values, Operators.ADD), part2),
                TestEquation(target, PerformOperation(values, Operators.MUL), part2)
            };

            if (part2)
            {
                subsequentTests.Add(TestEquation(target, PerformOperation(values, Operators.CON), part2));
            }

            return subsequentTests.Any((b) => b);
        }

        private static long[] PerformOperation(long[] values, Operators op)
        {
            long[] result = new long[values.Length - 1];
            if (op == Operators.ADD) result[0] = values[0] + values[1];
            else if (op == Operators.MUL) result[0] = values[0] * values[1];
            else if (op == Operators.CON) result[0] = long.Parse(values[0].ToString() + values[1].ToString());

            for (int i = 2; i < values.Length; i++)
            {
                result[i - 1] = values[i];
            }

            return result;
        }

    }
}
