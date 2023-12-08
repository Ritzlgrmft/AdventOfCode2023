namespace AdventOfCode2023.Day5a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var lines = File.ReadLines(inputFile).ToArray();
        var seeds = lines[0][7..].Split(" ").Select(n => long.Parse(n)).ToList();

        var startIndex = 3;
        var maps = new List<List<Mapping>>();
        while (startIndex < lines.Length)
        {
            var map = ReadMap(lines, startIndex);
            maps.Add(map);
            startIndex += map.Count + 2;
        }

        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var location = seed;
            foreach (var map in maps)
            {
                foreach (var mapping in map)
                {
                    if (mapping.sourceStart <= location && location < mapping.sourceStart + mapping.length)
                    {
                        location = mapping.destinationStart + location - mapping.sourceStart;
                        break;
                    }
                }
            }
            locations.Add(location);
        }

        return locations.Min();
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

struct Mapping
{
    public long destinationStart;
    public long sourceStart;
    public long length;
}
