using System.Data;

namespace AdventOfCode2023.Day10b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read map
        var routingMap = new List<List<PipeSymbol>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var row = line.ToCharArray().Select(c => (PipeSymbol)c).ToList();
            routingMap.Add(row);
        }

        // find start
        var start = FindStart(routingMap);

        // find start direction
        List<Direction> startDirections = new List<Direction>(); ;
        if (start.Row > 0 && Array.IndexOf(new[] { PipeSymbol.SouthWest, PipeSymbol.NorthSouth, PipeSymbol.SouthEast }, routingMap[start.Row - 1][start.Col]) != -1)
        {
            startDirections.Add(Direction.North);
        }
        if (start.Col > 0 && Array.IndexOf(new[] { PipeSymbol.SouthEast, PipeSymbol.EastWest, PipeSymbol.NorthEast }, routingMap[start.Row][start.Col - 1]) != -1)
        {
            startDirections.Add(Direction.West);
        }
        if (start.Row < routingMap.Count - 1 && Array.IndexOf(new[] { PipeSymbol.NorthWest, PipeSymbol.NorthSouth, PipeSymbol.NorthEast }, routingMap[start.Row + 1][start.Col]) != -1)
        {
            startDirections.Add(Direction.South);
        }
        if (start.Col < routingMap[0].Count - 1 && Array.IndexOf(new[] { PipeSymbol.SouthWest, PipeSymbol.EastWest, PipeSymbol.NorthWest }, routingMap[start.Row][start.Col + 1]) != -1)
        {
            startDirections.Add(Direction.East);
        }

        // initialize pipeMap
        var pipeMap = new bool[routingMap.Count * 2, routingMap[0].Count * 2];
        PipeSymbol startRoute;
        if (startDirections.Intersect(new[] { Direction.North, Direction.South }).Count() == 2)
        {
            startRoute = PipeSymbol.NorthSouth;
        }
        else if (startDirections.Intersect(new[] { Direction.North, Direction.West }).Count() == 2)
        {
            startRoute = PipeSymbol.NorthWest;
        }
        else if (startDirections.Intersect(new[] { Direction.North, Direction.East }).Count() == 2)
        {
            startRoute = PipeSymbol.NorthEast;
        }
        else if (startDirections.Intersect(new[] { Direction.West, Direction.East }).Count() == 2)
        {
            startRoute = PipeSymbol.EastWest;
        }
        else if (startDirections.Intersect(new[] { Direction.West, Direction.South }).Count() == 2)
        {
            startRoute = PipeSymbol.SouthWest;
        }
        else
        {
            startRoute = PipeSymbol.SouthEast;
        }
        MarkPipeMap(pipeMap, startRoute, start);

        // follow pipe
        var pos = start;
        var direction = startDirections.First();
        var steps = 0;
        while (pos != start || steps == 0)
        {
            switch (direction)
            {
                case Direction.North:
                    pos.Row--;
                    if (routingMap[pos.Row][pos.Col] == PipeSymbol.SouthWest)
                    {
                        direction = Direction.West;
                    }
                    else if (routingMap[pos.Row][pos.Col] == PipeSymbol.SouthEast)
                    {
                        direction = Direction.East;
                    }
                    break;
                case Direction.East:
                    pos.Col++;
                    if (routingMap[pos.Row][pos.Col] == PipeSymbol.SouthWest)
                    {
                        direction = Direction.South;
                    }
                    else if (routingMap[pos.Row][pos.Col] == PipeSymbol.NorthWest)
                    {
                        direction = Direction.North;
                    }
                    break;
                case Direction.South:
                    pos.Row++;
                    if (routingMap[pos.Row][pos.Col] == PipeSymbol.NorthWest)
                    {
                        direction = Direction.West;
                    }
                    else if (routingMap[pos.Row][pos.Col] == PipeSymbol.NorthEast)
                    {
                        direction = Direction.East;
                    }
                    break;
                case Direction.West:
                    pos.Col--;
                    if (routingMap[pos.Row][pos.Col] == PipeSymbol.SouthEast)
                    {
                        direction = Direction.South;
                    }
                    else if (routingMap[pos.Row][pos.Col] == PipeSymbol.NorthEast)
                    {
                        direction = Direction.North;
                    }
                    break;
            }
            MarkPipeMap(pipeMap, routingMap[pos.Row][pos.Col], pos);

            steps++;
        }

        var freeFields = new List<(int Row, int Col)>();
        for (var row = 0; row < pipeMap.GetLength(0); row++)
        {
            if (!pipeMap[row, 0])
            {
                freeFields.Add((row, 0));
            }
            if (!pipeMap[row, pipeMap.GetLength(1) - 1])
            {
                freeFields.Add((row, pipeMap.GetLength(1) - 1));
            }
        }
        for (var col = 1; col < pipeMap.GetLength(1) - 1; col++)
        {
            if (!pipeMap[0, col])
            {
                freeFields.Add((0, col));
            }
            if (!pipeMap[pipeMap.GetLength(0) - 1, col])
            {
                freeFields.Add((pipeMap.GetLength(0) - 1, col));
            }
        }

        var freedFields = freeFields;
        while (freedFields.Any())
        {
            freedFields = freedFields.SelectMany(f => GetFreeNeighbours(pipeMap, f)).Except(freeFields).ToList();
            freeFields.AddRange(freedFields);
        }

        var enclosedFields = 0;
        for (var row = 2; row < pipeMap.GetLength(0) - 2; row += 2)
        {
            for (var col = 2; col < pipeMap.GetLength(1) - 2; col += 2)
            {
                if (!pipeMap[row, col] && !freeFields.Any(f => f.Row == row && f.Col == col))
                {
                    enclosedFields++;
                }
            }
        }

        return enclosedFields;
    }

    private IEnumerable<(int Row, int Col)> GetFreeNeighbours(bool[,] pipeMap, (int Row, int Col) pos)
    {
        if (pos.Row > 0 && !pipeMap[pos.Row - 1, pos.Col])
        {
            yield return (pos.Row - 1, pos.Col);
        }
        if (pos.Row < pipeMap.GetLength(0) - 1 && !pipeMap[pos.Row + 1, pos.Col])
        {
            yield return (pos.Row + 1, pos.Col);
        }
        if (pos.Col > 0 && !pipeMap[pos.Row, pos.Col - 1])
        {
            yield return (pos.Row, pos.Col - 1);
        }
        if (pos.Col < pipeMap.GetLength(1) - 1 && !pipeMap[pos.Row, pos.Col + 1])
        {
            yield return (pos.Row, pos.Col + 1);
        }
    }

    private void MarkPipeMap(bool[,] pipeMap, PipeSymbol routing, (int Row, int Col) pos)
    {
        pipeMap[pos.Row * 2, pos.Col * 2] = true;
        switch (routing)
        {
            case PipeSymbol.NorthSouth:
                pipeMap[pos.Row * 2 + 1, pos.Col * 2] = true;
                break;
            case PipeSymbol.EastWest:
                pipeMap[pos.Row * 2, pos.Col * 2 + 1] = true;
                break;
            case PipeSymbol.NorthEast:
                pipeMap[pos.Row * 2, pos.Col * 2 + 1] = true;
                break;
            case PipeSymbol.NorthWest:
                break;
            case PipeSymbol.SouthWest:
                pipeMap[pos.Row * 2 + 1, pos.Col * 2] = true;
                break;
            case PipeSymbol.SouthEast:
                pipeMap[pos.Row * 2 + 1, pos.Col * 2] = true;
                pipeMap[pos.Row * 2, pos.Col * 2 + 1] = true;
                break;
        }

    }

    private static (int Row, int Col) FindStart(List<List<PipeSymbol>> routingMap)
    {
        for (var row = 0; row < routingMap.Count; row++)
        {
            var index = routingMap[row].IndexOf(PipeSymbol.Start);
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