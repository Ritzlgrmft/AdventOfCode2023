using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace AdventOfCode2023.Day19a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var workflows = new Dictionary<string, List<(string? Condition, string Target)>>();
        var parts = new List<Part>();
        var parseWorkflow = true;
        foreach (var line in File.ReadLines(inputFile))
        {
            if (string.IsNullOrEmpty(line))
            {
                parseWorkflow = false;
            }
            else
            {
                if (parseWorkflow)
                {
                    var lineParts = line.Split(new char[] { '{', '}', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var conditions = new List<(string? Condition, string Target)>();
                    for (var i = 1; i < lineParts.Length - 2; i += 2)
                    {
                        conditions.Add((lineParts[i], lineParts[i + 1]));
                    }
                    conditions.Add((null, lineParts.Last()));
                    workflows[lineParts[0]] = conditions;
                }
                else
                {
                    var lineParts = line.Split(new char[] { '{', '}', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var part = new Part { x = int.Parse(lineParts[1]), m = int.Parse(lineParts[3]), a = int.Parse(lineParts[5]), s = int.Parse(lineParts[7]) };
                    parts.Add(part);
                }
            }
        }

        var sum = 0L;
        foreach (var part in parts)
        {
            var currentWorkflow = "in";
            while (currentWorkflow != "A" && currentWorkflow != "R")
            {
                foreach (var rule in workflows[currentWorkflow])
                {
                    if (EvaluateCondition(rule.Condition, part).Result)
                    {
                        currentWorkflow = rule.Target;
                        break;
                    }
                }
            }
            if (currentWorkflow == "A")
            {
                sum += part.x + part.m + part.a + part.s;
            }
        }

        return sum;
    }

    public record Part
    {
        public int x;
        public int m;
        public int a;
        public int s;
    }

    private async Task<bool> EvaluateCondition(string? condition, Part part)
    {
        return string.IsNullOrEmpty(condition) || await CSharpScript.EvaluateAsync<bool>(condition, globals: part);
    }
}