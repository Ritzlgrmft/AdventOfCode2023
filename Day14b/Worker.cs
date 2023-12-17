



namespace AdventOfCode2023.Day14b;

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

        var history = new List<string>();
        var iterations = 1000000000;
        for (var cycle = 0; cycle < iterations; cycle++)
        {
            RollNorth(rocks);
            RollWest(rocks);
            RollSouth(rocks);
            RollEast(rocks);

            var currentHistory = BuildHistory(rocks);
            var historyIndex = history.IndexOf(currentHistory);
            if (historyIndex == -1)
            {
                history.Add(currentHistory);
            }
            else
            {
                var rest = (iterations - historyIndex - 1) % (cycle - historyIndex);
                rocks = BuildRocksFromHistory(history[historyIndex + rest]);
                break;
            }
        }
        var sum = CalculateNorthWeight(rocks);

        return sum;
    }

    private List<char[]> BuildRocksFromHistory(string currentHistory)
    {
        var rocks = new List<char[]>();
        foreach (var line in currentHistory.Split("\n"))
        {
            rocks.Add(line.ToCharArray());
        }
        return rocks;
    }

    private string BuildHistory(List<char[]> rocks)
    {
        return string.Join("\n", rocks.Select(r => new string(r)));
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

    private void RollWest(List<char[]> rocks)
    {
        for (var col = 1; col < rocks[0].Length; col++)
        {
            for (var row = 0; row < rocks.Count; row++)
            {
                if (rocks[row][col] == ROUNDED_ROCK)
                {
                    var col2 = col - 1;
                    while (col2 >= 0 && rocks[row][col2] == EMPTY_SPACE)
                    {
                        rocks[row][col2] = ROUNDED_ROCK;
                        rocks[row][col2 + 1] = EMPTY_SPACE;
                        col2--;
                    }
                }
            }
        }
    }

    private void RollSouth(List<char[]> rocks)
    {
        for (var row = rocks.Count - 2; row >= 0; row--)
        {
            for (var col = 0; col < rocks[0].Length; col++)
            {
                if (rocks[row][col] == ROUNDED_ROCK)
                {
                    var row2 = row + 1;
                    while (row2 < rocks.Count && rocks[row2][col] == EMPTY_SPACE)
                    {
                        rocks[row2][col] = ROUNDED_ROCK;
                        rocks[row2 - 1][col] = EMPTY_SPACE;
                        row2++;
                    }
                }
            }
        }

    }

    private void RollEast(List<char[]> rocks)
    {
        for (var col = rocks[0].Length - 2; col >= 0; col--)
        {
            for (var row = 0; row < rocks.Count; row++)
            {
                if (rocks[row][col] == ROUNDED_ROCK)
                {
                    var col2 = col + 1;
                    while (col2 < rocks[0].Length && rocks[row][col2] == EMPTY_SPACE)
                    {
                        rocks[row][col2] = ROUNDED_ROCK;
                        rocks[row][col2 - 1] = EMPTY_SPACE;
                        col2++;
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

