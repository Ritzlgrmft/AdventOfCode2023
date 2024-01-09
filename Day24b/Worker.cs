
using System.Data;

namespace AdventOfCode2023.Day24b;

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

        // find hailstones with same vx
        var hailstonesGroupedByVx = hailstones.GroupBy(h => h.vx).Select(g => (g.Key, g.Count())).Where(hg => hg.Item2 > 1).Select(hg => hg.Item1);
        var potentialRx = Enumerable.Range(-1000, 2001).ToList();
        foreach (var vx in hailstonesGroupedByVx)
        {
            var hailstonesToCheck = hailstones.Where(h => h.vx == vx).ToList();
            var nextPotentialRx = new List<int>();
            foreach (var prx in potentialRx)
            {
                if (prx != vx && Math.Abs(hailstonesToCheck[0].px - hailstonesToCheck[1].px) % Math.Abs(prx - vx) == 0)
                {
                    nextPotentialRx.Add(prx);
                }
            }
            potentialRx = nextPotentialRx;
        }
        var rx = potentialRx[0];

        // find hailstones with same vy
        var hailstonesGroupedByVy = hailstones.GroupBy(h => h.vy).Select(g => (g.Key, g.Count())).Where(hg => hg.Item2 > 1).Select(hg => hg.Item1);
        var potentialRy = Enumerable.Range(-1000, 2001).ToList();
        foreach (var vy in hailstonesGroupedByVy)
        {
            var hailstonesToCheck = hailstones.Where(h => h.vy == vy).ToList();
            var nextPotentialRy = new List<int>();
            foreach (var pry in potentialRy)
            {
                if (pry != vy && Math.Abs(hailstonesToCheck[0].py - hailstonesToCheck[1].py) % Math.Abs(pry - vy) == 0)
                {
                    nextPotentialRy.Add(pry);
                }
            }
            potentialRy = nextPotentialRy;
        }
        var ry = potentialRy[0];

        // find hailstones with same vz
        var hailstonesGroupedByVz = hailstones.GroupBy(h => h.vz).Select(g => (g.Key, g.Count())).Where(hg => hg.Item2 > 1).Select(hg => hg.Item1);
        var potentialRz = Enumerable.Range(-1000, 2001).ToList();
        foreach (var vz in hailstonesGroupedByVz)
        {
            var hailstonesToCheck = hailstones.Where(h => h.vz == vz).ToList();
            var nextPotentialRz = new List<int>();
            foreach (var prz in potentialRz)
            {
                if (prz != vz && Math.Abs(hailstonesToCheck[0].pz - hailstonesToCheck[1].pz) % Math.Abs(prz - vz) == 0)
                {
                    nextPotentialRz.Add(prz);
                }
            }
            potentialRz = nextPotentialRz;
        }
        var rz = potentialRz[0];

        // calculate intersection
        var h1 = hailstones[0];
        var h2 = hailstones[1];
        var ma = (double)(h1.vy - ry) / (h1.vx - rx);
        var mb = (double)(h2.vy - ry) / (h2.vx - rx);
        var ca = h1.py - (ma * h1.px);
        var cb = h2.py - (mb * h2.px);
        var x = (long)((cb - ca) / (ma - mb));
        var y = (long)(ma * x + ca);
        var t = (x - h1.px) / (h1.vx - rx);
        var z = h1.pz + (h1.vz - rz) * t;

        return x + y + z;
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