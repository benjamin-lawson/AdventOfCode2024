namespace AdventOfCode2024
{
    public class Program
    {
        static void Main(string[] args)
        {
            ISolution solution = new Day3.Solution();
            string[] testData = File.ReadAllLines("Day3/testdata_part1.txt");
            string[] testDataPart2 = File.ReadAllLines("Day3/testdata_part2.txt");
            string[] inputData = File.ReadAllLines("Day3/input.txt");

            Console.WriteLine("===  Part 1  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart1(testData);

            Console.WriteLine("Real Solution");
            solution.SolvePart1(inputData);
            
            Console.WriteLine("===  Part 2  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart2(testDataPart2);

            Console.WriteLine("Real Solution");
            solution.SolvePart2(inputData);
            
        }
    }
}
