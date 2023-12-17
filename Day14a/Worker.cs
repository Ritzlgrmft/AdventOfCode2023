
namespace AdventOfCode2023.Day14a;

public class Worker : IWorker
{
    private const char ROUNDED_ROCK = 'O';
    private const char EMPTY_SPACE = '.';

    public long DoWork(string inputFile)
    {
        // read input
        var rocks = new List<char[]>();
        foreach (var line in File.ReadLines(inputFile))
        {
            rocks.Add(line.ToCharArray());
        }

        RollNorth(rocks);
        var sum = CalculateNorthWeight(rocks);

        return sum;
    }

    private void RollNorth(List<char[]> rocks)
    {
        for (var row = 1; row < rocks.Count; row++)
        {
            for (var col = 0; col < rocks[0].Length; col++)
            {
                if (rocks[row][col] == ROUNDED_ROCK)
                {
                    var row2 = row - 1;
                    while (row2 >= 0 && rocks[row2][col] == EMPTY_SPACE)
                    {
                        rocks[row2][col] = ROUNDED_ROCK;
                        rocks[row2 + 1][col] = EMPTY_SPACE;
                        row2--;
                    }
                }
            }
        }
    }

    private long CalculateNorthWeight(List<char[]> rocks)
    {
        var sum = 0L;
        for (var row = 0; row < rocks.Count; row++)
        {
            sum += rocks[row].Count(r => r == ROUNDED_ROCK) * (rocks.Count - row);
        }
        return sum;
    }
}

