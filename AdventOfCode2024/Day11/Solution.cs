using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day11
{
    
    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            List<long> stones = inputData[0].Split().Select(long.Parse).ToList();
            Dictionary<long, long> stoneMap = stones.GroupBy(s => s).ToDictionary(g => g.Key, g => g.LongCount());

            int blinkNumber = 0;
            while (blinkNumber < 25)
            {
                stoneMap = ApplyStoneRules(stoneMap);
                blinkNumber++;
                Console.WriteLine($"Blink {blinkNumber}: {stoneMap.Sum(kv => kv.Value)}");
            }

            Console.WriteLine($"Total Stones: {stoneMap.Sum(kv => kv.Value)}");
        }

        public Dictionary<long, long> ApplyStoneRules(Dictionary<long, long> stones)
        {
            Dictionary<long, long> newStones = new Dictionary<long, long>();

            foreach (var kv in stones) 
            { 
                long stone = kv.Key;
                
                if (stone == 0)
                {
                    SafeAdd(newStones, 1, kv.Value);
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    long newStone1 = long.Parse(stone.ToString().Substring(0, stone.ToString().Length / 2));
                    SafeAdd(newStones, newStone1, kv.Value);

                    long newStone2 = long.Parse(stone.ToString().Substring(stone.ToString().Length / 2));
                    SafeAdd(newStones, newStone2, kv.Value);
                }
                else
                {
                    SafeAdd(newStones, stone * 2024, kv.Value);
                }
            }

            return newStones;
        }

        public void SafeAdd(Dictionary<long, long> stoneMap, long key, long amount)
        {
            if (stoneMap.ContainsKey(key)) stoneMap[key] += amount;
            else stoneMap[key] = amount;
        }

        public void SolvePart2(string[] inputData)
        {
            List<long> stones = inputData[0].Split().Select(long.Parse).ToList();
            Dictionary<long, long> stoneMap = stones.GroupBy(s => s).ToDictionary(g => g.Key, g => g.LongCount());

            int blinkNumber = 0;
            while (blinkNumber < 75)
            {
                stoneMap = ApplyStoneRules(stoneMap);
                blinkNumber++;
                Console.WriteLine($"Blink {blinkNumber}: {stoneMap.Sum(kv => kv.Value)}");
            }

            Console.WriteLine($"Total Stones: {stoneMap.Sum(kv => kv.Value)}");
        }
    }
}
