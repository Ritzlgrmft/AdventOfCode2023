namespace AdventOfCode2023.Day21a;

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

        for (var step = 0; step < 64; step++)
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

        return positions.Count;

        void AddIfPossible(List<(int row, int col)> newPositions, int row, int col)
        {
            if (0 <= row && row < map.Count && 0 <= col && col < map[0].Count && !map[row][col] && !newPositions.Contains((row, col)))
            {
                newPositions.Add((row, col));
            }
        }
    }

}