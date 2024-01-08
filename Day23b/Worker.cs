using System.Data;

namespace AdventOfCode2023.Day23b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var map = File.ReadLines(inputFile).Select(l => l.ToCharArray()).ToArray();
        var start = new Position(row: 0, Array.IndexOf(map[0], '.'));
        var target = new Position(map.Length - 1, Array.IndexOf(map[^1], '.'));

        // compress graph
        var graph = new List<(Position start, Position end, int length)>();
        var nodesToAnalyze = new Queue<(Position, Position, List<Position>)>();
        nodesToAnalyze.Enqueue((start, start with { row = start.row + 1 }, new List<Position>()));
        while (nodesToAnalyze.Any())
        {
            var (current, next, visitedNodes) = nodesToAnalyze.Dequeue();
            start = current;
            visitedNodes.Add(current);
            var neighbours = GetNeighbours(next, visitedNodes);
            var length = 1;
            while (neighbours.Count == 1)
            {
                (current, next) = (next, neighbours.Except(new Position[] { current }).First());
                length++;
                visitedNodes.Add(current);
                neighbours = GetNeighbours(next, visitedNodes);
            }

            if (neighbours.Count >= 1 || next.row == map.Length - 1)
            {
                if (!graph.Any(g => IsSameGraph((g.start, g.end), (start, next))))
                {
                    graph.Add((start, next, length));
                    foreach (var neighbour in neighbours)
                    {
                        nodesToAnalyze.Enqueue((next, neighbour, visitedNodes.Select(v => v).ToList()));
                    }
                }
            }
        }

        var maxHike = 0;
        var states = new Stack<(Position pos, List<Position> visited, int distance)>();
        states.Push((new Position(row: 0, Array.IndexOf(map[0], '.')), new List<Position>(), 0));
        while (states.Count > 0)
        {
            var (pos, visited, distance) = states.Pop();

            if (pos.row == target.row)
            {
                maxHike = Math.Max(distance, maxHike);
            }
            else
            {
                visited.Add(pos);
                var nextPositions = graph.Where(g => g.start.row == pos.row && g.start.col == pos.col).ToList();
                var nextPositionsReverse = graph
                    .Where(g => g.end.row == pos.row && g.end.col == pos.col)
                    .Select(g => (start: g.end, end: g.start, MaxLengthAttribute: g.length))
                    .ToList();
                nextPositions = nextPositions
                    .Union(nextPositionsReverse)
                    .Where(g => !visited.Contains(g.end))
                    .ToList();

                foreach (var nextPos in nextPositions)
                {
                    states.Push((nextPos.end, visited.Select(v => v).ToList(), distance + nextPos.length));
                }
            }
        }

        return maxHike;

        bool IsSameGraph((Position start, Position end) g1, (Position start, Position end) g2)
        {
            return (g1.start.row == g2.start.row && g1.start.col == g2.start.col && g1.end.row == g2.end.row && g1.end.col == g2.end.col)
                || (g1.start.row == g2.end.row && g1.start.col == g2.end.col && g1.end.row == g2.start.row && g1.end.col == g2.start.col);
        }

        List<Position> GetNeighbours(Position pos, List<Position> visitedNodes)
        {
            var neigbours = new List<Position>();
            AddNeighbourIfPossible(neigbours, visitedNodes, pos.row, pos.col - 1);
            AddNeighbourIfPossible(neigbours, visitedNodes, pos.row, pos.col + 1);
            AddNeighbourIfPossible(neigbours, visitedNodes, pos.row - 1, pos.col);
            AddNeighbourIfPossible(neigbours, visitedNodes, pos.row + 1, pos.col);
            return neigbours;
        }

        void AddNeighbourIfPossible(List<Position> neigbours, List<Position> visitedNodes, int row, int col)
        {
            var pos = new Position(row, col);
            if (row < map.Length && map[row][col] != '#' && !visitedNodes.Contains(pos))
            {
                neigbours.Add(pos);
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