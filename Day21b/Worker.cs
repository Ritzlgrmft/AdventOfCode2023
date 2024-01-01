namespace AdventOfCode2023.Day21b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var map = new List<List<bool>>();
        var row = 0;
        var positions = new List<(int row, int col)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            map.Add(line.ToCharArray().Select(c => c == '#').ToList());
            var startCol = line.IndexOf('S');
            if (startCol != -1)
            {
                positions.Add((row, startCol));
            }
            row++;
        }

        var numbers = new List<long>(){
            CalculateSteps( positions, 65),
            CalculateSteps( positions, 65 + 131),
            CalculateSteps( positions, 65 + 131 + 131)
        };

        var sequence = new List<List<long>>() { numbers };
        for (var steps = 2; steps < 202300; steps++)
        {
            // build deltas
            while (sequence.Last().Any(n => n != 0))
            {
                var lastHistory = sequence.Last();
                var newHistory = new List<long>();
                for (var i = 1; i < lastHistory.Count; i++)
                {
                    newHistory.Add(lastHistory[i] - lastHistory[i - 1]);
                }
                sequence.Add(newHistory);
            }

            // enhance sequence
            var lastValue = 0L;
            sequence.Last().Add(lastValue);
            for (var i = sequence.Count - 2; i >= 0; i--)
            {
                lastValue += sequence[i].Last();
                sequence[i].Add(lastValue);
            }
        }

        return sequence[0].Last();

        long CalculateSteps(List<(int row, int col)> positions, long steps)
        {
            for (var step = 0; step < steps; step++)
            {
                var newPositions = new List<(int row, int col)>();
                foreach (var pos in positions)
                {
                    AddIfPossible(newPositions, pos.row - 1, pos.col);
                    AddIfPossible(newPositions, pos.row + 1, pos.col);
                    AddIfPossible(newPositions, pos.row, pos.col - 1);
                    AddIfPossible(newPositions, pos.row, pos.col + 1);
                }
                positions = newPositions;
            }

            Console.WriteLine(positions.Count);
            return positions.Count;
        }

        void AddIfPossible(List<(int row, int col)> newPositions, int row, int col)
        {
            if (!map[(row + 262) % 131][(col + 262) % 131] && !newPositions.Contains((row, col)))
            {
                newPositions.Add((row, col));
            }
        }

    }

}