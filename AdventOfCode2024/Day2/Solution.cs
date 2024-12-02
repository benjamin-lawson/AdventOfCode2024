using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day2
{
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            int validReports = 0;
            foreach (string reportData in inputData)
            {
                var reportValues = reportData.Split(" ").Select(x => int.Parse(x)).ToList();
                validReports += IsValidReport(reportValues) ? 1 : 0;
            }
            Console.WriteLine($"Valid Reports: {validReports}");
        }

        private bool IsValidReport(List<int> reportValues)
        {
            bool initialDirection = reportValues[0] - reportValues[1] >= 0;
            int prevValue = reportValues[0];

            for (int i = 1; i < reportValues.Count; i++)
            {
                int diff = prevValue - reportValues[i];
                if (diff >= 0 != initialDirection)
                {
                    return false;
                }

                diff = Math.Abs(diff);
                if (diff < 1 || diff > 3)
                {
                    return false;
                }

                prevValue = reportValues[i];
            }

            return true;
        }

        private bool IsValidDampendedReport(List<int> reportValues) 
        {
            for (int i = 0; i < reportValues.Count; i++) 
            { 
                List<int> reportCopy = new List<int>(reportValues);
                reportCopy.RemoveAt(i);
                if (IsValidReport(reportCopy))
                {
                    return true;
                }
            }
            return false;
        }

        public void SolvePart2(string[] inputData)
        {
            int validReports = 0;
            foreach (string reportData in inputData)
            {
                var reportValues = reportData.Split(" ").Select(x => int.Parse(x)).ToList();
                bool isValid = IsValidReport(reportValues);

                if (isValid)
                {
                    validReports++;
                    continue;
                }

                bool isValidDampended = IsValidDampendedReport(reportValues);
                if (isValidDampended)
                {
                    validReports++;
                }
            }
            Console.WriteLine($"Valid Reports: {validReports}");
        }
    }
}
