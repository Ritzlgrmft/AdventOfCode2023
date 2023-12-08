namespace AdventOfCode2023.Day8a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var lines = File.ReadLines(inputFile);
        var instructions = lines.First().ToCharArray();

        var elements = new Dictionary<string, (string, string)>();
        foreach (var line in lines.Skip(2))
        {
            var lineParts = line.Split(new string[] { " = (", ", ", ")" }, StringSplitOptions.RemoveEmptyEntries);
            elements[lineParts[0]] = (lineParts[1], lineParts[2]);
        }

        var pos = "AAA";
        var instruction = 0;
        var steps = 0;
        while (pos != "ZZZ")
        {
            var element = elements[pos];
            pos = instructions[instruction] == 'L' ? element.Item1 : element.Item2;
            steps++;
            instruction++;
            if (instruction == instructions.Length)
            {
                instruction = 0;
            }
        }
        return steps;
    }
}
