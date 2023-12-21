namespace AdventOfCode2023.Day16b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var tiles = new List<List<char>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            tiles.Add(line.ToCharArray().ToList());
        }

        var max = 0L;
        for (var row = 0; row < tiles.Count; row++)
        {
            max = Math.Max(max, GetEnergization(tiles, (row, -1, 90)));
            max = Math.Max(max, GetEnergization(tiles, (row, tiles[row].Count, 270)));
        }
        for (var col = 0; col < tiles[0].Count; col++)
        {
            max = Math.Max(max, GetEnergization(tiles, (-1, col, 180)));
            max = Math.Max(max, GetEnergization(tiles, (tiles.Count, col, 0)));
        }
        return max;
    }

    private static int GetEnergization(List<List<char>> tiles, (int Row, int Col, int Direction) start)
    {
        var beams = new List<(int Row, int Col, int Direction)>() { start };
        var beamsCache = new List<(int Row, int Col, int Direction)>() { start };
        var energized = new bool[tiles.Count, tiles[0].Count];
        while (beams.Count > 0)
        {
            var newBeams = new List<(int, int, int)>();
            foreach (var beam in beams)
            {
                if (beam.Direction == 0 && beam.Row > 0)
                {
                    switch (tiles[beam.Row - 1][beam.Col])
                    {
                        case '\\':
                            newBeams.Add((beam.Row - 1, beam.Col, 270));
                            break;
                        case '/':
                            newBeams.Add((beam.Row - 1, beam.Col, 90));
                            break;
                        case '-':
                            newBeams.Add((beam.Row - 1, beam.Col, 90));
                            newBeams.Add((beam.Row - 1, beam.Col, 270));
                            break;
                        default:
                            newBeams.Add((beam.Row - 1, beam.Col, 0));
                            break;
                    }
                }
                else if (beam.Direction == 90 && beam.Col < tiles[0].Count - 1)
                {
                    switch (tiles[beam.Row][beam.Col + 1])
                    {
                        case '\\':
                            newBeams.Add((beam.Row, beam.Col + 1, 180));
                            break;
                        case '/':
                            newBeams.Add((beam.Row, beam.Col + 1, 0));
                            break;
                        case '|':
                            newBeams.Add((beam.Row, beam.Col + 1, 0));
                            newBeams.Add((beam.Row, beam.Col + 1, 180));
                            break;
                        default:
                            newBeams.Add((beam.Row, beam.Col + 1, 90));
                            break;
                    }
                }
                else if (beam.Direction == 180 && beam.Row < tiles.Count - 1)
                {
                    switch (tiles[beam.Row + 1][beam.Col])
                    {
                        case '\\':
                            newBeams.Add((beam.Row + 1, beam.Col, 90));
                            break;
                        case '/':
                            newBeams.Add((beam.Row + 1, beam.Col, 270));
                            break;
                        case '-':
                            newBeams.Add((beam.Row + 1, beam.Col, 90));
                            newBeams.Add((beam.Row + 1, beam.Col, 270));
                            break;
                        default:
                            newBeams.Add((beam.Row + 1, beam.Col, 180));
                            break;
                    }
                }
                else if (beam.Direction == 270 && beam.Col > 0)
                {
                    switch (tiles[beam.Row][beam.Col - 1])
                    {
                        case '\\':
                            newBeams.Add((beam.Row, beam.Col - 1, 0));
                            break;
                        case '/':
                            newBeams.Add((beam.Row, beam.Col - 1, 180));
                            break;
                        case '|':
                            newBeams.Add((beam.Row, beam.Col - 1, 0));
                            newBeams.Add((beam.Row, beam.Col - 1, 180));
                            break;
                        default:
                            newBeams.Add((beam.Row, beam.Col - 1, 270));
                            break;
                    }
                }
            }

            var reallyNewBeams = newBeams.Where(b => !beamsCache.Contains(b)).ToList();
            beams = reallyNewBeams;
            beamsCache.AddRange(reallyNewBeams);

            foreach (var beam in beams)
            {
                energized[beam.Row, beam.Col] = true;
            }
        }

        var sum = 0;
        for (var row = 0; row < tiles.Count; row++)
        {
            for (var col = 0; col < tiles[0].Count; col++)
            {
                if (energized[row, col])
                {
                    sum++;
                }
            }
        }
        return sum;
    }
}