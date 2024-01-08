
using System.Data;

namespace AdventOfCode2023.Day23a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var map = File.ReadLines(inputFile).Select(l => l.ToCharArray()).ToArray();
        var start = new Position(row: 0, Array.IndexOf(map[0], '.'));
        var target = new Position(map.Length - 1, Array.IndexOf(map[^1], '.'));

        var hikes = FindHikes(start, new List<Position>());
        return hikes.Max();

        List<int> FindHikes(Position pos, List<Position> visited)
        {
            var hikes = new List<int>();
            while (pos.row != target.row)
            {
                visited.Add(pos);
                var nextPositions = new List<Position>();
                switch (map[pos.row][pos.col])
                {
                    case '>':
                        AddIfPossible(nextPositions, pos.row, pos.col + 1, visited);
                        break;
                    case '<':
                        AddIfPossible(nextPositions, pos.row, pos.col - 1, visited);
                        break;
                    case 'v':
                        AddIfPossible(nextPositions, pos.row + 1, pos.col, visited);
                        break;
                    case '^':
                        AddIfPossible(nextPositions, pos.row - 1, pos.col, visited);
                        break;
                    default:
                        AddIfPossible(nextPositions, pos.row, pos.col - 1, visited);
                        AddIfPossible(nextPositions, pos.row, pos.col + 1, visited);
                        AddIfPossible(nextPositions, pos.row - 1, pos.col, visited);
                        AddIfPossible(nextPositions, pos.row + 1, pos.col, visited);
                        break;
                }

                if (nextPositions.Count == 0)
                {
                    return hikes;
                }

                pos = nextPositions.First();
                foreach (var nextPos in nextPositions.Skip(1))
                {
                    hikes.AddRange(FindHikes(nextPos, visited.Select(v => v).ToList()));
                }
            }

            hikes.Add(visited.Count);
            return hikes;
        }

        void AddIfPossible(List<Position> nextPositions, int row, int col, List<Position> visited)
        {
            var pos = new Position(row, col);
            if (row >= 0 && map[row][col] != '#' && !visited.Contains(pos))
            {
                nextPositions.Add(pos);
            }
        }
    }
}

internal struct Position
{
    public int row;
    public int col;

    public Position(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other &&
               row == other.row &&
               col == other.col;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(row, col);
    }

    public void Deconstruct(out int row, out int col)
    {
        row = this.row;
        col = this.col;
    }

    public override string ToString()
    {
        return $"({row},{col})";
    }
}