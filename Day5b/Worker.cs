namespace AdventOfCode2023.Day5b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var lines = File.ReadLines(inputFile).ToArray();
        var seedNumbers = lines[0][7..].Split(" ").Select(n => long.Parse(n)).ToList();
        var seeds = new List<Seed>();
        for (var i = 0; i < seedNumbers.Count; i += 2)
        {
            var seed = new Seed { start = seedNumbers[i], length = seedNumbers[i + 1] };
            seeds.Add(seed);
        }

        var startIndex = 3;
        var maps = new List<List<Mapping>>();
        while (startIndex < lines.Length)
        {
            var map = ReadMap(lines, startIndex);
            maps.Add(map);
            startIndex += map.Count + 2;
        }

        foreach (var map in maps)
        {
            var seedsForNextMap = new List<Seed>();
            var seedIndex = 0;
            while (seedIndex < seeds.Count)
            {
                var seed = seeds[seedIndex];
                var isSeedProcessed = false;
                foreach (var mapping in map)
                {
                    if (seed.end > mapping.sourceStart && seed.start < mapping.sourceEnd)
                    {
                        // seed and mapping overlap
                        if (seed.start < mapping.sourceStart)
                        {
                            // seed is starting further left
                            var newSeed = new Seed { start = seed.start, length = mapping.sourceStart - seed.start };
                            seeds.Add(newSeed);
                            seed.start = mapping.sourceStart;
                            seed.length -= newSeed.length;
                        }
                        if (seed.end > mapping.sourceEnd)
                        {
                            // seed is ending further right
                            var newSeed = new Seed { start = mapping.sourceEnd, length = seed.end - mapping.sourceEnd };
                            seeds.Add(newSeed);
                            seed.length -= newSeed.length;

                        }
                        var seedForNextMap = new Seed { start = mapping.destinationStart - mapping.sourceStart + seed.start, length = seed.length };
                        seedsForNextMap.Add(seedForNextMap);
                        isSeedProcessed = true;
                        break;
                    }
                }

                if (!isSeedProcessed)
                {
                    seedsForNextMap.Add(seed);
                }
                seedIndex++;
            }
            seeds = seedsForNextMap;
        }

        return seeds.Min(s => s.start);
    }

    private List<Mapping> ReadMap(string[] lines, int startIndex)
    {
        var map = new List<Mapping>();
        for (var index = startIndex; index < lines.Length && lines[index] != ""; index++)
        {
            var lineParts = lines[index].Split(" ").Select(n => long.Parse(n)).ToArray();
            map.Add(new Mapping { destinationStart = lineParts[0], sourceStart = lineParts[1], length = lineParts[2] });
        }
        return map;
    }
}

struct Seed
{
    public long start;
    public long length;
    public readonly long end
    {
        get
        {
            return start + length;
        }
    }
}

struct Mapping
{
    public long destinationStart;
    public long sourceStart;
    public long length;
    public readonly long sourceEnd
    {
        get
        {
            return sourceStart + length;
        }
    }
}
