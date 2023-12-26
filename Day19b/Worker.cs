namespace AdventOfCode2023.Day19b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var workflows = new Dictionary<string, List<(string? Condition, string Target)>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var lineParts = line.Split(new char[] { '{', '}', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var conditions = new List<(string? Condition, string Target)>();
            for (var i = 1; i < lineParts.Length - 2; i += 2)
            {
                conditions.Add((lineParts[i], lineParts[i + 1]));
            }
            conditions.Add((null, lineParts.Last()));
            workflows[lineParts[0]] = conditions;
        }

        var part = new Part { xMin = 1, xMax = 4000, mMin = 1, mMax = 4000, aMin = 1, aMax = 4000, sMin = 1, sMax = 4000 };
        var acceptedParts = AnalyzeWorkflow("in", new List<Part>() { part });

        var sum = 0L;
        foreach (var acceptedPart in acceptedParts)
        {
            sum +=
               (long)(acceptedPart.xMax - acceptedPart.xMin + 1)
                   * (acceptedPart.mMax - acceptedPart.mMin + 1)
                   * (acceptedPart.aMax - acceptedPart.aMin + 1)
                   * (acceptedPart.sMax - acceptedPart.sMin + 1);
        }

        return sum;

        List<Part> AnalyzeWorkflow(string workflow, List<Part> parts)
        {
            var acceptedParts = new List<Part>();
            if (parts.Count > 0)
            {
                foreach (var rule in workflows[workflow])
                {
                    var (TrueParts, FalseParts) = AnalyzeCondition(rule.Condition, parts);

                    if (rule.Target == "R")
                    {
                        // dispose trueParts
                    }
                    else if (rule.Target == "A")
                    {
                        acceptedParts.AddRange(TrueParts);
                    }
                    else
                    {
                        acceptedParts.AddRange(AnalyzeWorkflow(rule.Target, TrueParts));
                    }

                    // falseParts to be continued with next check
                    if (FalseParts.Count > 0)
                    {
                        parts = FalseParts;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return acceptedParts;
        }

        (List<Part> TrueParts, List<Part> FalseParts) AnalyzeCondition(string? condition, List<Part> parts)
        {
            var trueParts = new List<Part>();
            var falseParts = new List<Part>();
            foreach (var part in parts)
            {
                if (condition == null)
                {
                    trueParts.Add(part);
                }
                else
                {
                    var value = int.Parse(condition.Substring(2));
                    if (condition[0] == 'x')
                    {
                        if (condition[1] == '<')
                        {
                            if (value > part.xMax)
                            {
                                trueParts.Add(part);
                            }
                            else if (value <= part.xMin)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { xMax = value - 1 });
                                falseParts.Add(part with { xMin = value });
                            }
                        }
                        else
                        {
                            if (value < part.xMin)
                            {
                                trueParts.Add(part);
                            }
                            else if (value >= part.xMax)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { xMin = value + 1 });
                                falseParts.Add(part with { xMax = value });
                            }

                        }
                    }
                    else if (condition[0] == 'm')
                    {
                        if (condition[1] == '<')
                        {
                            if (value > part.mMax)
                            {
                                trueParts.Add(part);
                            }
                            else if (value <= part.mMin)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { mMax = value - 1 });
                                falseParts.Add(part with { mMin = value });
                            }
                        }
                        else
                        {
                            if (value < part.mMin)
                            {
                                trueParts.Add(part);
                            }
                            else if (value >= part.mMax)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { mMin = value + 1 });
                                falseParts.Add(part with { mMax = value });
                            }

                        }
                    }
                    else if (condition[0] == 'a')
                    {
                        if (condition[1] == '<')
                        {
                            if (value > part.aMax)
                            {
                                trueParts.Add(part);
                            }
                            else if (value <= part.aMin)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { aMax = value - 1 });
                                falseParts.Add(part with { aMin = value });
                            }
                        }
                        else
                        {
                            if (value < part.aMin)
                            {
                                trueParts.Add(part);
                            }
                            else if (value >= part.aMax)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { aMin = value + 1 });
                                falseParts.Add(part with { aMax = value });
                            }

                        }
                    }
                    else if (condition[0] == 's')
                    {
                        if (condition[1] == '<')
                        {
                            if (value > part.sMax)
                            {
                                trueParts.Add(part);
                            }
                            else if (value <= part.sMin)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { sMax = value - 1 });
                                falseParts.Add(part with { sMin = value });
                            }
                        }
                        else
                        {
                            if (value < part.sMin)
                            {
                                trueParts.Add(part);
                            }
                            else if (value >= part.sMax)
                            {
                                falseParts.Add(part);
                            }
                            else
                            {
                                trueParts.Add(part with { sMin = value + 1 });
                                falseParts.Add(part with { sMax = value });
                            }

                        }
                    }
                }
            }
            return (trueParts, falseParts);
        }
    }

    public record Part
    {
        public int xMin;
        public int xMax;
        public int mMin;
        public int mMax;
        public int aMin;
        public int aMax;
        public int sMin;
        public int sMax;
    }
}