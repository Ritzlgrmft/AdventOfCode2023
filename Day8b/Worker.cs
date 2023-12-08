namespace AdventOfCode2023.Day8b;

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

        var elementPaths = new Dictionary<string, (List<int>, string)>();
        foreach (var element in elements.Keys)
        {
            var newElement = element;
            var endIterations = new List<int>();
            for (var instruction = 0; instruction < instructions.Length; instruction++)
            {
                newElement = instructions[instruction] == 'L' ? elements[newElement].Item1 : elements[newElement].Item2;
                if (newElement.EndsWith('Z'))
                {
                    endIterations.Add(instruction + 1);
                }
            }
            elementPaths[element] = (endIterations, newElement);
        }

        var rounds = new List<long>();
        foreach (var startPos in elements.Keys.Where(k => k.EndsWith('A')))
        {
            var pos = startPos;
            var round = 0;
            do
            {
                var positionPath = elementPaths[pos];
                pos = positionPath.Item2;
                round++;
            }
            while (!pos.EndsWith('Z'));
            rounds.Add(round);
        }

        return rounds.Aggregate((long)instructions.Length, (a, b) => a * b);
    }

}
