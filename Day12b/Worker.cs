using System.Data;

namespace AdventOfCode2023.Day12b;

public class Worker : IWorker
{

    Dictionary<string, long> cache = new Dictionary<string, long>();

    public long DoWork(string inputFile)
    {

        var springMap = new List<(string Springs, int[] ContigousGroupLengths)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(" ");
            var springs = lineParts[0] + "?" + lineParts[0] + "?" + lineParts[0] + "?" + lineParts[0] + "?" + lineParts[0];
            var lengths = lineParts[1] + "," + lineParts[1] + "," + lineParts[1] + "," + lineParts[1] + "," + lineParts[1];
            springMap.Add((springs, lengths.Split(",").Select(int.Parse).ToArray()));
        }

        var sum = 0L;
        foreach (var springEntry in springMap)
        {
            var numberOfMatchingSprings = GetNumberOfMatchingSpringsWithCache(springEntry.Springs, springEntry.ContigousGroupLengths);
            sum += numberOfMatchingSprings;
        }

        return sum;
    }

    private long GetNumberOfMatchingSpringsWithCache(string input, int[] targetLengths)
    {
        var cacheKey = input + " " + string.Join(",", targetLengths);
        if (cache.ContainsKey(cacheKey))
        {
            return cache[cacheKey];
        }

        var value = GetNumberOfMatchingSprings(input, targetLengths);
        cache[cacheKey] = value;
        return value;
    }

    private long GetNumberOfMatchingSprings(string input, int[] targetLengths)
    {
        var index = 0;
        while (index < input.Length && input[index] == '.')
        {
            index++;
        }

        if (index == input.Length)
        {
            return targetLengths.Length == 0 ? 1 : 0;
        }

        var springStart = index;
        while (index < input.Length && input[index] == '#')
        {
            index++;
        }

        if (index == input.Length)
        {
            if (targetLengths.Length == 1 && index - springStart == targetLengths[0])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        if (input[index] == '.')
        {
            if (targetLengths.Length > 0 && index - springStart == targetLengths[0])
            {
                return GetNumberOfMatchingSpringsWithCache(input[index..], targetLengths.Skip(1).ToArray());
            }
            else
            {
                return 0;
            }
        }
        else
        {
            var result = 0L;
            if (targetLengths.Length > 0 && index - springStart == targetLengths[0])
            {
                result += GetNumberOfMatchingSpringsWithCache("." + input[(index + 1)..], targetLengths.Skip(1).ToArray());
            }
            else
            {
                if (targetLengths.Length > 0 && index - springStart < targetLengths[0])
                {
                    result += GetNumberOfMatchingSpringsWithCache(input[..index] + "#" + input[(index + 1)..], targetLengths);
                }
                if (index == springStart)
                {
                    result += GetNumberOfMatchingSpringsWithCache(input[(index + 1)..], targetLengths);
                }
            }
            return result;
        }

    }
}

