namespace AdventOfCode2023.Day15a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var steps = File.ReadAllText(inputFile).Split(",", StringSplitOptions.RemoveEmptyEntries);

        var sum = 0L;
        foreach (var step in steps)
        {
            sum += CalculateHash(step);
        }

        return sum;
    }

    private long CalculateHash(string input)
    {
        var result = 0L;
        foreach (var c in input.ToCharArray())
        {
            result += c;
            result *= 17;
            result %= 256;
        }
        return result;
    }
}

