
using System.Data;

namespace AdventOfCode2023.Day24a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var hailstones = new List<Hailstone>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(new char[] { ',', '@' }, StringSplitOptions.TrimEntries).Select(n => long.Parse(n)).ToList();
            var hailstone = new Hailstone { px = lineParts[0], py = lineParts[1], pz = lineParts[2], vx = lineParts[3], vy = lineParts[4], vz = lineParts[5] };
            hailstones.Add(hailstone);
        }

        var minX = 200000000000000;
        var maxX = 400000000000000;
        var minY = 200000000000000;
        var maxY = 400000000000000;

        // find intersections
        // m1 * x + n1 == m2 * x + n2
        // x == (n2 - n1) / (m1 - m2)
        var numberOfInsideIntersections = 0;
        for (var i = 0; i < hailstones.Count; i++)
        {
            for (var j = i + 1; j < hailstones.Count; j++)
            {
                var h1 = hailstones[i];
                var h2 = hailstones[j];

                if (h1.m == h2.m)
                {
                    // parallel
                }
                else
                {
                    var x = (h2.n - h1.n) / (h1.m - h2.m);
                    var y = h1.m * x + h1.n;

                    if ((h1.vx > 0 && x < h1.px) || (h1.vx < 0 && x > h1.px) || (h2.vx > 0 && x < h2.px) || (h2.vx < 0 && x > h2.px))
                    {
                        // past
                    }
                    else if (x < minX || maxX < x || y < minY || y > maxY)
                    {
                        // outside
                    }
                    else
                    {
                        // inside"
                        numberOfInsideIntersections++;
                    }
                }
            }
        }

        return numberOfInsideIntersections;
    }
}

struct Hailstone
{
    public long px;
    public long py;
    public long pz;
    public long vx;
    public long vy;
    public long vz;

    // y = mx + n
    public double m { get { return (double)vy / vx; } }
    public double n { get { return py - m * px; } }

    public override string ToString()
    {
        return $"{px}, {py}, {pz} @ {vx}, {vy}, {vz}";
    }
}