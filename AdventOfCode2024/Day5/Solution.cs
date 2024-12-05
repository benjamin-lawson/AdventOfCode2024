using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day5
{
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            var pageData = BuildPageData(
                inputData.Where(
                    l => l.Contains('|')
                )
            );

            var validPageOrders = GetValidPageOrders(
                inputData.Where(
                    l => l.Contains(',')
                ),
                pageData
            );

            int totalSum = 0;
            foreach (var order in validPageOrders)
            {
                totalSum += int.Parse(order[order.Length / 2]);
            }
            Console.WriteLine($"Total: {totalSum}");
        }

        private Dictionary<string, List<string>> BuildPageData(IEnumerable<string> inputData)
        {
            Dictionary<string, List<string>> pageData = new Dictionary<string, List<string>>();
            foreach (string line in inputData)
            {
                string[] pageSplit = line.Split('|');
                if (!pageData.ContainsKey(pageSplit[0]))
                {
                    pageData[pageSplit[0]] = new List<string>() { pageSplit[1] };
                }
                else if (!pageData[pageSplit[0]].Contains(pageSplit[1]))
                {
                    pageData[pageSplit[0]].Add(pageSplit[1]);
                }
            }
            return pageData;
        }

        private IEnumerable<string[]> GetValidPageOrders(IEnumerable<string> inputData, Dictionary<string, List<string>> pageData)
            => inputData.Select(l => l.Split(',')).Where(x => IsValidPageOrder(x, pageData));

        private bool IsValidPageOrder(string[] pageOrder, Dictionary<string, List<string>> pageData)
        {
            for (int i = 0; i < pageOrder.Length; i++)
            {
                string item = pageOrder[i];
                if (!pageData.ContainsKey(item)) continue;

                foreach (string rulePage in pageData[item])
                {
                    if (!pageOrder.Contains(rulePage)) continue;

                    if (i > Array.IndexOf(pageOrder, rulePage))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void SolvePart2(string[] inputData)
        {
            var pageData = BuildPageData(
                inputData.Where(
                    l => l.Contains('|')
                )
            );

            var invalidPageOrders = GetInvalidPageOrders(
                inputData.Where(
                    l => l.Contains(',')
                ),
                pageData
            );

            var correctedPageOrders = CorrectInvalidPageOrders(invalidPageOrders, pageData);

            int totalSum = 0;
            foreach (var order in correctedPageOrders)
            {
                totalSum += int.Parse(order[order.Length / 2]);
            }
            Console.WriteLine($"Total: {totalSum}");
        }

        private IEnumerable<string[]> GetInvalidPageOrders(IEnumerable<string> inputData, Dictionary<string, List<string>> pageData)
            => inputData.Select(l => l.Split(',')).Where(x => !IsValidPageOrder(x, pageData));

        private IEnumerable<string[]> CorrectInvalidPageOrders(IEnumerable<string[]> inputData, Dictionary<string, List<string>> pageData)
        {
            foreach (var order in inputData)
            {
                Array.Sort(order, (i1, i2) =>
                {
                    if (pageData.ContainsKey(i1) && pageData[i1].Contains(i2))
                    {
                        return 1;
                    }

                    if (pageData.ContainsKey(i2) && pageData[i2].Contains(i1))
                    {
                        return -1;
                    }

                    return 0;
                });

                yield return order;
            }
        }
    }
    
}
