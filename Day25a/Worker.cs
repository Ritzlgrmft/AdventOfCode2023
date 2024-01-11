
using System.Data;

namespace AdventOfCode2023.Day25a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var components = new List<string>();
        var connections = new List<(string, string)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            components.AddRange(lineParts);
            connections.AddRange(lineParts.Skip(1).Select(c => (lineParts[0], c)));
            connections.AddRange(lineParts.Skip(1).Select(c => (c, lineParts[0])));
        }
        components = components.Distinct().ToList();
        connections = connections.Distinct().ToList();

        var part1 = components.Select(c => c).ToList();
        var (numberOfConnections, componentWithMostConnections) = CountConnectionsToPart2(part1, connections);
        while (numberOfConnections != 3)
        {
            part1.Remove(componentWithMostConnections);
            (numberOfConnections, componentWithMostConnections) = CountConnectionsToPart2(part1, connections);
        }


        return part1.Count * (components.Count - part1.Count);
    }

    private (int numberOfConnections, string componentWithMostConnections) CountConnectionsToPart2(List<string> part1, List<(string, string)> connections)
    {
        var connectionsToPart2 = new Dictionary<string, int>();
        foreach (var component in part1)
        {
            connectionsToPart2[component] = connections.Where(c => c.Item1 == component && !part1.Contains(c.Item2)).Count();
        }
        return (connectionsToPart2.Values.Sum(), connectionsToPart2.Keys.First(k => connectionsToPart2[k] == connectionsToPart2.Values.Max()));
    }
}
