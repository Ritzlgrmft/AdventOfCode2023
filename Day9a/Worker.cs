namespace AdventOfCode2023.Day9a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read sequences
        var sequences = new List<List<List<int>>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            sequences.Add(new List<List<int>>() {
                line.Split(" ").Select(n=>int.Parse(n)).ToList()
            });
        }

        // build deltas
        foreach (var sequence in sequences)
        {
            while (sequence.Last().Any(n => n != 0))
            {
                var lastHistory = sequence.Last();
                var newHistory = new List<int>();
                for (var i = 1; i < lastHistory.Count; i++)
                {
                    newHistory.Add(lastHistory[i] - lastHistory[i - 1]);
                }
                sequence.Add(newHistory);
            }
        }

        // enhance sequences
        foreach (var sequence in sequences)
        {
            var lastValue = 0;
            sequence.Last().Add(lastValue);
            for (var i = sequence.Count - 2; i >= 0; i--)
            {
                lastValue += sequence[i].Last();
                sequence[i].Add(lastValue);
            }
        }

        return sequences.Sum(s => s[0].Last());
    }
}
