namespace AdventOfCode2023.Day1b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var numberStrings = new Dictionary<string, int>()
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
            ["1"] = 1,
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9
        };

        var sum = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var positions = numberStrings.Keys.Select(n => (n, AllIndexesOf(line, n))).Where(i => i.Item2.Any()).ToArray();

            var minPos = positions.Min(p => p.Item2.First());
            var minDigit = positions.First(p => p.Item2.Contains(minPos)).Item1;
            var firstDigit = numberStrings[minDigit];

            var maxPos = positions.Max(p => p.Item2.Last());
            var maxDigit = positions.First(p => p.Item2.Contains(maxPos)).Item1;
            var lastDigit = numberStrings[maxDigit];

            var number = firstDigit * 10 + lastDigit;
            sum += number;
        }
        return sum;
    }

    int[] AllIndexesOf(string str, string searchstring)
    {
        var result = new List<int>();
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            result.Add(minIndex);
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
        }
        return result.ToArray();
    }
}
