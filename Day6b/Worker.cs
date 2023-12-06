using System.Dynamic;

namespace AdventOfCode2023.Day6b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var lines = File.ReadLines(inputFile).ToArray();
        var durations = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
        var records = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();

        var duration = long.Parse(string.Join("", durations));
        var record = long.Parse(string.Join("", records));

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

        return (end - start + 1);
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
