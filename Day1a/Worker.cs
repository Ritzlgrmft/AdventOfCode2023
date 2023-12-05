namespace AdventOfCode2023.Day1a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var sum = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var firstDigit = line.ToCharArray().First(c => char.IsDigit(c));
            var lastDigit = line.ToCharArray().Last(c => char.IsDigit(c));
            var number = int.Parse(new string(new char[] { firstDigit, lastDigit }));
            sum += number;
        }
        return sum;
    }
}
