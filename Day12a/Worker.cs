using System.Data;

namespace AdventOfCode2023.Day12a;

public class Worker : IWorker
{

    public long DoWork(string inputFile)
    {

        var springMap = new List<(char[] Springs, int[] ContigousGroupLengths)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(" ");
            springMap.Add((lineParts[0].ToCharArray(), lineParts[1].Split(",").Select(n => int.Parse(n)).ToArray()));
        }

        var sum = 0L;
        foreach (var springEntry in springMap)
        {
            var potentialSprings = GetPotentialSprings(springEntry.Springs);
            foreach (var potentialSpring in potentialSprings)
            {
                if (AreLengthsEqual(GetContigousLengths(potentialSpring), springEntry.ContigousGroupLengths))
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    private int[] GetContigousLengths(char[] springs)
    {
        return springs.GroupWhile((a, b) => a == b).Where(g => g.First() == '#').Select(g => g.Count()).ToArray();
    }

    private bool AreLengthsEqual(int[] a, int[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }

    private List<char[]> GetPotentialSprings(char[] input)
    {
        if (!input.Any(c => c == '?'))
        {
            return new List<char[]>() { input };
        }

        var i = 0;
        while (input[i] != '?')
        {
            i++;
        }
        var variant1 = input.Select(c => c).ToArray();
        variant1[i] = '.';
        var variant2 = input.Select(c => c).ToArray();
        variant2[i] = '#';
        return GetPotentialSprings(variant1).Union(GetPotentialSprings(variant2)).ToList();
    }

}

public static class LinqExtension
{
    public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> seq, Func<T, T, bool> condition)
    {
        T prev = seq.First();
        List<T> list = new List<T>() { prev };

        foreach (T item in seq.Skip(1))
        {
            if (condition(prev, item) == false)
            {
                yield return list;
                list = new List<T>();
            }
            list.Add(item);
            prev = item;
        }

        yield return list;
    }
}