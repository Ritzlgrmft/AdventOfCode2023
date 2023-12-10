using System.Data;

namespace AdventOfCode2023.Day10a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read map
        var pipeMap = new List<List<PipeSymbol>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var row = line.ToCharArray().Select(c => (PipeSymbol)c).ToList();
            pipeMap.Add(row);
        }

        // find start
        var start = FindStart(pipeMap);

        // find start direction
        Direction direction;
        if (start.Row > 0 && Array.IndexOf(new[] { PipeSymbol.SouthWest, PipeSymbol.NorthSouth, PipeSymbol.SouthEast }, pipeMap[start.Row - 1][start.Col]) != -1)
        {
            direction = Direction.North;
        }
        else if (start.Col > 0 && Array.IndexOf(new[] { PipeSymbol.SouthEast, PipeSymbol.EastWest, PipeSymbol.NorthEast }, pipeMap[start.Row][start.Col - 1]) != -1)
        {
            direction = Direction.West;
        }
        else if (start.Row < pipeMap.Count - 1 && Array.IndexOf(new[] { PipeSymbol.NorthWest, PipeSymbol.NorthSouth, PipeSymbol.NorthEast }, pipeMap[start.Row + 1][start.Col]) != -1)
        {
            direction = Direction.South;
        }
        else
        {
            direction = Direction.East;
        }

        // follow pipe
        var steps = 0;
        var pos = start;
        while (pos != start || steps == 0)
        {
            switch (direction)
            {
                case Direction.North:
                    pos.Row--;
                    if (pipeMap[pos.Row][pos.Col] == PipeSymbol.SouthWest)
                    {
                        direction = Direction.West;
                    }
                    else if (pipeMap[pos.Row][pos.Col] == PipeSymbol.SouthEast)
                    {
                        direction = Direction.East;
                    }
                    break;
                case Direction.East:
                    pos.Col++;
                    if (pipeMap[pos.Row][pos.Col] == PipeSymbol.SouthWest)
                    {
                        direction = Direction.South;
                    }
                    else if (pipeMap[pos.Row][pos.Col] == PipeSymbol.NorthWest)
                    {
                        direction = Direction.North;
                    }
                    break;
                case Direction.South:
                    pos.Row++;
                    if (pipeMap[pos.Row][pos.Col] == PipeSymbol.NorthWest)
                    {
                        direction = Direction.West;
                    }
                    else if (pipeMap[pos.Row][pos.Col] == PipeSymbol.NorthEast)
                    {
                        direction = Direction.East;
                    }
                    break;
                case Direction.West:
                    pos.Col--;
                    if (pipeMap[pos.Row][pos.Col] == PipeSymbol.SouthEast)
                    {
                        direction = Direction.South;
                    }
                    else if (pipeMap[pos.Row][pos.Col] == PipeSymbol.NorthEast)
                    {
                        direction = Direction.North;
                    }
                    break;
            }

            steps++;
        }

        return steps / 2;
    }

    private static (int Row, int Col) FindStart(List<List<PipeSymbol>> pipeMap)
    {
        for (var row = 0; row < pipeMap.Count; row++)
        {
            var index = pipeMap[row].IndexOf(PipeSymbol.Start);
            if (index != -1)
            {
                return (row, index);
            }
        }
        return (-1, -1);
    }

    enum PipeSymbol
    {
        NorthSouth = '|',
        EastWest = '-',
        NorthEast = 'L',
        NorthWest = 'J',
        SouthWest = '7',
        SouthEast = 'F',
        Ground = '.',
        Start = 'S'
    }

    enum Direction
    {
        North,
        East,
        South,
        West
    }
}