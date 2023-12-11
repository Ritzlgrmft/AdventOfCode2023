using System.Data;

namespace AdventOfCode2023.Day11a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read map
        var map = new List<List<char>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var row = line.ToCharArray().ToList();
            map.Add(row);
        }

        // find empty rows
        var emptyRows = new List<int>();
        for (var row = 0; row < map.Count; row++)
        {
            if (map[row].All(c => c == '.'))
            {
                emptyRows.Add(row);
            }
        }

        // find empty cols
        var emptyCols = new List<int>();
        for (var col = 0; col < map[0].Count; col++)
        {
            if (map.Select(row => row[col]).All(c => c == '.'))
            {
                emptyCols.Add(col);
            }
        }

        // find galaxies
        var galaxies = new List<(int Row, int Col)>();
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[0].Count; col++)
            {
                if (map[row][col] == '#')
                {
                    galaxies.Add((row, col));
                }
            }
        }

        // calculate paths
        var sum = 0;
        for (var a = 0; a < galaxies.Count; a++)
        {
            var galaxyA = galaxies[a];
            for (var b = a + 1; b < galaxies.Count; b++)
            {
                var galaxyB = galaxies[b];
                var minRow = Math.Min(galaxyA.Row, galaxyB.Row);
                var maxRow = Math.Max(galaxyA.Row, galaxyB.Row);
                var minCol = Math.Min(galaxyA.Col, galaxyB.Col);
                var maxCol = Math.Max(galaxyA.Col, galaxyB.Col);
                var emptyRowsInBetween = emptyRows.Count(r => minRow < r && r < maxRow);
                var emptyColsInBetween = emptyCols.Count(c => minCol < c && c < maxCol);
                var dist = maxRow - minRow + emptyRowsInBetween + maxCol - minCol + emptyColsInBetween;
                sum += dist;
            }

        }

        return sum;
    }

}