
namespace AdventOfCode2023.Day22b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var bricks = new List<Brick>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(',', '~');
            var brick = new Brick(
                new Position(int.Parse(lineParts[0]), int.Parse(lineParts[1]), int.Parse(lineParts[2])),
                new Position(int.Parse(lineParts[3]), int.Parse(lineParts[4]), int.Parse(lineParts[5]))
            );
            bricks.Add(brick);
        }

        // move bricks down
        var zToAnalyze = 1;
        while (bricks.Any(b => b.start.z > zToAnalyze))
        {
            // find bricks above
            var bricksAbove = bricks.Where(b => b.start.z == zToAnalyze + 1).ToList();

            // find bricks and blocked positions in current z
            var bricksCurrent = bricks.Where(b => Enumerable.Range(b.start.z, b.end.z - b.start.z + 1).Contains(zToAnalyze)).ToList();
            var blockedPositions = new List<(int x, int y)>();
            foreach (var brick in bricksCurrent)
            {
                for (var x = brick.start.x; x <= brick.end.x; x++)
                {
                    for (var y = brick.start.y; y <= brick.end.y; y++)
                    {
                        blockedPositions.Add((x, y));
                    }
                }
            }

            var brickMoved = false;
            for (var i = 0; i < bricksAbove.Count; i++)
            {
                var brick = bricksAbove[i];
                var brickFreeToMove = true;
                for (var x = brick.start.x; x <= brick.end.x && brickFreeToMove; x++)
                {
                    for (var y = brick.start.y; y <= brick.end.y && brickFreeToMove; y++)
                    {
                        if (blockedPositions.Contains((x, y)))
                        {
                            brickFreeToMove = false;
                        }
                    }
                }

                if (brickFreeToMove)
                {
                    brick.start.z--;
                    brick.end.z--;
                    brickMoved = true;
                }
            }

            if (brickMoved)
            {
                zToAnalyze = Math.Max(zToAnalyze - 1, 1);
            }
            else
            {
                zToAnalyze++;
            }
        }

        var supportingBricks = new List<(int brick, List<int> supporting)>();
        for (var i = 0; i < bricks.Count; i++)
        {
            var brick = bricks[i];

            var supporting = bricks
                .Select((b, i) => (brick: b, index: i))
                .Where(bi => bi.brick.start.z == brick.end.z + 1 &&
                    bi.brick.end.x >= brick.start.x && bi.brick.start.x <= brick.end.x &&
                    bi.brick.end.y >= brick.start.y && bi.brick.start.y <= brick.end.y)
                .Select(bi => bi.index)
                .ToList();
            supportingBricks.Add((i, supporting));
        }

        var sum = 0L;
        for (var i = 0; i < bricks.Count; i++)
        {
            var fallingBricks = GetFallingBricks(i, new List<int>() { i });
            sum += fallingBricks;
            Console.WriteLine($"{i}: {fallingBricks} => {sum}");
        }

        return sum;

        int GetFallingBricks(int index, List<int> removedBricks)
        {
            var result = 0;
            foreach (var supported in supportingBricks.First(sb => sb.brick == index).supporting)
            {
                var supportedBy = supportingBricks.Where(sb => sb.supporting.Contains(supported)).Select(sb => sb.brick);
                if (!supportedBy.Except(removedBricks).Any())
                {
                    removedBricks.Add(supported);
                    result += 1 + GetFallingBricks(supported, removedBricks);
                }
            }
            return result;
        }
    }

}

internal class Position
{
    public int x;
    public int y;
    public int z;

    public Position(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other &&
               x == other.x &&
               y == other.y &&
               z == other.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }

    public static implicit operator (int x, int y, int z)(Position value)
    {
        return (value.x, value.y, value.z);
    }

    public static implicit operator Position((int x, int y, int z) value)
    {
        return new Position(value.x, value.y, value.z);
    }
}

internal class Brick
{
    public Position start;
    public Position end;

    public Brick(Position start, Position end)
    {
        this.start = start;
        this.end = end;
    }

    public override bool Equals(object? obj)
    {
        return obj is Brick other &&
               EqualityComparer<Position>.Default.Equals(start, other.start) &&
               EqualityComparer<Position>.Default.Equals(end, other.end);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(start, end);
    }

    public void Deconstruct(out Position start, out Position end)
    {
        start = this.start;
        end = this.end;
    }

    public static implicit operator (Position start, Position end)(Brick value)
    {
        return (value.start, value.end);
    }

    public static implicit operator Brick((Position start, Position end) value)
    {
        return new Brick(value.start, value.end);
    }

    public override string ToString()
    {
        return $"{start.x},{start.y},{start.z}~{end.x},{end.y},{end.z}";
    }
}