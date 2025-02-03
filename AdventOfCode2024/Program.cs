namespace AdventOfCode2024
{
    public class Program
    {
        static void Main(string[] args)
        {
            ISolution solution = new Day15.Solution();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string day = solution.GetType().Namespace.Replace("AdventOfCode2024.", "");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            string[] testData = File.ReadAllLines($"{day}/testdata.txt");
            string[] testDataPart2 = File.ReadAllLines($"{day}/testdata.txt");
            string[] inputData = File.ReadAllLines($"{day}/input.txt");
            string[] inputDataPart2 = File.ReadAllLines($"{day}/input.txt");

            Console.WriteLine("===  Part 1  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart1(testData);

            Console.WriteLine("Real Solution");
            solution.SolvePart1(inputData);
            
            Console.WriteLine("===  Part 2  ===");
            Console.WriteLine("Test Solution");
            solution.SolvePart2(testDataPart2);

            Console.WriteLine("Real Solution");
            solution.SolvePart2(inputDataPart2);
            
        }
    }
}
