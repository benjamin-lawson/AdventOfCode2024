using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode2024.Utilities;

namespace AdventOfCode2024.Day13
{
    public class ClawMachine
    {
        public int AX; // 94
        public int AY; // 34

        public int BX; // 22
        public int BY; // 67

        public long XTarget; // 8400
        public long YTarget; // 5400

        // X = 80
        // Y = 40

        public const int ButtonACost = 3;
        public const int ButtonBCost = 1;
        public const int MaxButtonPush = 100;

        public double TestX()
        {
            var v1 = AX * BY * 1.0;
            var v2 = XTarget * BY * 1.0;

            var v3 = AY * BX;
            var v4 = YTarget * BX;

            return (v2 - v4) / (v1 - v3);
        }

        public double TestY(double x)
        {
            var v1 = AY * x;
            return (YTarget - v1) / BY;
        }

        public double SolveForX() => ((XTarget * BY * 1.0) - (YTarget * BX)) / ((AX * BY * 1.0) - (BX * AY));

        public double SolveForY(double x) => (XTarget - (AX * x)) / BX;

        public bool TryGetTotalTokens(out int tokens)
        {
            tokens = 0;
            double x = SolveForX();
            double y = SolveForY(x);

            if (x % 1 != 0 || y % 1 != 0) return false;

            tokens = ((int)x * ButtonACost) + ((int)y * ButtonBCost);

            return !(x > MaxButtonPush || y > MaxButtonPush);
        }

        public bool TryGetTotalTokensPart2(out long tokens)
        {
            tokens = 0;
            double x = SolveForX();
            double y = SolveForY(x);

            if (x % 1 != 0 || y % 1 != 0) return false;

            tokens = ((long)x * ButtonACost) + ((long)y * ButtonBCost);

            return true;
        }
    }

    public class Solution : ISolution
    {
        private Regex _inputRegex = new Regex(@"^(?<control>Button [AB]|Prize): X[+=](?<x>\d+), Y[+=](?<y>\d+)$", RegexOptions.Compiled);
        public void SolvePart1(string[] inputData)
        {
            var machines = ParseInputToMachines(inputData);

            int totalTokens = 0;
            foreach (var machine in machines)
            {
                if (machine.TryGetTotalTokens(out var tokens))
                {
                    totalTokens += tokens;
                }
            }

            Console.WriteLine($"Total Tokens: {totalTokens}");
        }

        public List<ClawMachine> ParseInputToMachines(string[] inputData, long targetOffset = 0)
        {
            var machines = new List<ClawMachine>();

            foreach (string line in inputData)
            {
                var match = _inputRegex.Match(line);
                if (!match.Success) continue;

                string control = match.Groups.GetValueOrDefault("control").Value;
                if (control == "Button A")
                {
                    machines.Add(new ClawMachine
                    {
                        AX = int.Parse(match.Groups.GetValueOrDefault("x").Value),
                        AY = int.Parse(match.Groups.GetValueOrDefault("y").Value)
                    });
                }
                else if (control == "Button B")
                {
                    machines.Last().BX = int.Parse(match.Groups.GetValueOrDefault("x").Value);
                    machines.Last().BY = int.Parse(match.Groups.GetValueOrDefault("y").Value);
                }
                else if (control == "Prize")
                {
                    machines.Last().XTarget = int.Parse(match.Groups.GetValueOrDefault("x").Value) + targetOffset;
                    machines.Last().YTarget = int.Parse(match.Groups.GetValueOrDefault("y").Value) + targetOffset;
                }
            }

            return machines;
        }

        public void SolvePart2(string[] inputData)
        {
            var machines = ParseInputToMachines(inputData, 10000000000000);

            long totalTokens = 0;
            foreach (var machine in machines)
            {
                if (machine.TryGetTotalTokensPart2(out var tokens))
                {
                    totalTokens += tokens;
                }
            }

            Console.WriteLine($"Total Tokens: {totalTokens}");
        }
    }
}
