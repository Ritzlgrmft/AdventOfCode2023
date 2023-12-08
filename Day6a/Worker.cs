namespace AdventOfCode2023.Day6a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var lines = File.ReadLines(inputFile).ToArray();
        var durations = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(n => long.Parse(n)).ToList();
        var records = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(n => long.Parse(n)).ToList();

        var results = new List<long>();
        for (var race = 0; race < durations.Count; race++)
        {
            var duration = durations[race];
            var record = records[race];

            var start = 1;
            while ((duration - start) * start <= record)
            {
                start++;
            }

            var end = duration - 1;
            while ((duration - end) * end <= record)
            {
                end--;
            }

            results.Add(end - start + 1);
        }

        return results.Aggregate(1L, (a, b) => a * b);
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
