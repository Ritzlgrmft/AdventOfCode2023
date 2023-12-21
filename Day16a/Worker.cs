namespace AdventOfCode2023.Day16a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var tiles = new List<List<(char, bool)>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var tileRow = new List<(char, bool)>();
            foreach (var c in line.ToCharArray())
            {
                tileRow.Add((c, false));
            }
            tiles.Add(tileRow);
        }

        var beams = new List<(int Row, int Col, int Direction)>() { (0, -1, 90) };
        var beamsCache = new List<(int Row, int Col, int Direction)>() { (0, -1, 90) };
        while (beams.Count > 0)
        {
            var newBeams = new List<(int, int, int)>();
            foreach (var beam in beams)
            {
                if (beam.Direction == 0 && beam.Row > 0)
                {
                    switch (tiles[beam.Row - 1][beam.Col].Item1)
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
                    switch (tiles[beam.Row][beam.Col + 1].Item1)
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
                    switch (tiles[beam.Row + 1][beam.Col].Item1)
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
                    switch (tiles[beam.Row][beam.Col - 1].Item1)
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
                tiles[beam.Row][beam.Col] = (tiles[beam.Row][beam.Col].Item1, true);

            }
        }

        var sum = tiles.Sum(r => r.Count(t => t.Item2));
        return sum;
    }


}