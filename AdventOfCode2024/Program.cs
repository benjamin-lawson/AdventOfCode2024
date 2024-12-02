namespace AdventOfCode2024
{
    public class Program
    {
        static void Main(string[] args)
        {
            ISolution solution = new Day2.Solution();
            string[] testData = File.ReadAllLines("Day2/testdata.txt");
            string[] inputData = File.ReadAllLines("Day2/input.txt");

            Console.WriteLine("===  Part 1  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart1(testData);

            Console.WriteLine("Real Solution");
            solution.SolvePart1(inputData);
            
            Console.WriteLine("===  Part 2  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart2(testData);

            Console.WriteLine("Real Solution");
            solution.SolvePart2(inputData);
            
        }
    }
}
