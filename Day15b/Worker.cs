namespace AdventOfCode2023.Day15b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var steps = File.ReadAllText(inputFile).Split(",", StringSplitOptions.RemoveEmptyEntries);

        // initialize boxes
        var boxes = new List<List<(string, int)>>();
        for (var i = 0; i < 256; i++)
        {
            boxes.Add(new List<(string, int)>());
        }

        foreach (var step in steps)
        {
            var stepParts = step.Split(new char[] { '=', '-' });
            var label = stepParts[0];
            var boxIndex = CalculateHash(label);
            var box = boxes[boxIndex];
            var boxItemIndex = box.FindIndex(b => b.Item1 == label);
            if (step.IndexOf("-") > 0)
            {
                if (boxItemIndex != -1)
                {
                    box.RemoveAt(boxItemIndex);
                }
            }
            else if (step.IndexOf("=") > 0)
            {
                var focalLength = int.Parse(stepParts[1]);
                if (boxItemIndex != -1)
                {
                    box[boxItemIndex] = (label, focalLength);
                }
                else
                {
                    box.Add((label, focalLength));
                }
            }
        }

        var sum = 0L;
        for (var boxIndex = 0; boxIndex < 256; boxIndex++)
        {
            var box = boxes[boxIndex];
            for (var slot = 0; slot < box.Count; slot++)
            {
                var focusingPower = (boxIndex + 1) * (slot + 1) * box[slot].Item2;
                sum += focusingPower;
            }
        }
        return sum;
    }

    private int CalculateHash(string input)
    {
        var result = 0;
        foreach (var c in input.ToCharArray())
        {
            result += c;
            result *= 17;
            result %= 256;
        }
        return result;
    }
}

