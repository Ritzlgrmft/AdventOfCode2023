using System.Data;

namespace AdventOfCode2023.Day18a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var holes = new List<(int Row, int Col, string Color)>();
        var currentIndex = -1;
        var currentRow = 0;
        var currentCol = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            var direction = lineParts[0];
            var meters = int.Parse(lineParts[1]);
            var color = lineParts[2];

            for (var i = 0; i < meters; i++)
            {
                switch (lineParts[0])
                {
                    case "U":
                        holes.Add((currentRow - 2 * i - 1, currentCol, color));
                        holes.Add((currentRow - 2 * i - 2, currentCol, color));
                        break;
                    case "R":
                        holes.Add((currentRow, currentCol + 2 * i + 1, color));
                        holes.Add((currentRow, currentCol + 2 * i + 2, color));
                        break;
                    case "D":
                        holes.Add((currentRow + 2 * i + 1, currentCol, color));
                        holes.Add((currentRow + 2 * i + 2, currentCol, color));
                        break;
                    case "L":
                        holes.Add((currentRow, currentCol - 2 * i - 1, color));
                        holes.Add((currentRow, currentCol - 2 * i - 2, color));
                        break;
                }
            }
            currentIndex += 2 * meters;
            currentRow = holes[currentIndex].Row;
            currentCol = holes[currentIndex].Col;
        }
        Console.WriteLine($"Rows: {holes.Select(h => h.Row).Min()}-{holes.Select(h => h.Row).Max()}, Cols: {holes.Select(h => h.Col).Min()}-{holes.Select(h => h.Col).Max()}");

        // build map
        var minRow = holes.Select(h => h.Row).Min();
        var maxRow = holes.Select(h => h.Row).Max();
        var minCol = holes.Select(h => h.Col).Min();
        var maxCol = holes.Select(h => h.Col).Max();
        var map = new (bool Hole, string Color)[maxRow - minRow + 1, maxCol - minCol + 1];
        var x = map.GetLength(0);
        var y = map.GetLength(1);
        for (var row = minRow; row <= maxRow; row++)
        {
            for (var col = minCol; col <= maxCol; col++)
            {
                if (holes.Any(h => h.Row == row && h.Col == col))
                {
                    map[row - minRow, col - minCol] = (true, holes.FirstOrDefault(h => h.Row == row && h.Col == col).Color);
                }
            }
        }

        // fill interior
        var floodStack = new List<(int Row, int Col)>();
        for (var row = 1; row < map.GetLength(0) && floodStack.Count == 0; row++)
        {
            var col = 0;
            while (!map[row, col].Hole)
            {
                col++;
            }
            if (!map[row, col + 1].Hole)
            {
                floodStack.Add((row, col + 1));
            }
        }
        var iteration = 0L;
        while (floodStack.Count > 0)
        {
            var current = floodStack.Last();
            floodStack.Remove(current);

            map[current.Row, current.Col] = (true, "");

            if (!map[current.Row, current.Col - 1].Hole && !floodStack.Any(f => f.Row == current.Row && f.Col == current.Col - 1))
            {
                floodStack.Add((current.Row, current.Col - 1));
            }
            if (!map[current.Row, current.Col + 1].Hole && !floodStack.Any(f => f.Row == current.Row && f.Col == current.Col + 1))
            {
                floodStack.Add((current.Row, current.Col + 1));
            }
            if (!map[current.Row - 1, current.Col].Hole && !floodStack.Any(f => f.Row == current.Row - 1 && f.Col == current.Col))
            {
                floodStack.Add((current.Row - 1, current.Col));
            }
            if (!map[current.Row + 1, current.Col].Hole && !floodStack.Any(f => f.Row == current.Row + 1 && f.Col == current.Col))
            {
                floodStack.Add((current.Row + 1, current.Col));
            }
            iteration++;

            if (iteration % 1000 == 0)
            {
                Console.WriteLine($"{iteration}: {floodStack.Count} stack size");
            }
        }

        var sum = 0L;
        for (var row = 0; row < map.GetLength(0); row += 2)
        {
            for (var col = 0; col < map.GetLength(1); col += 2)
            {
                if (map[row, col].Hole)
                {
                    sum++;
                }
            }
        }
        return sum;
    }
}