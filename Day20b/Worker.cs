namespace AdventOfCode2023.Day20b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var modules = new Dictionary<string, (string type, string[] targets)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(" -> ");
            var targets = lineParts[1].Split(", ");
            if (lineParts[0] == "broadcaster")
            {
                modules[lineParts[0]] = ("broadcaster", targets);
            }
            else if (lineParts[0][0] == '%')
            {
                modules[lineParts[0][1..]] = ("flip-flop", targets);
            }
            else if (lineParts[0][0] == '&')
            {
                modules[lineParts[0][1..]] = ("conjunction", targets);
            }
        }

        var flipFlopStates = new Dictionary<string, bool>();
        var conjunctionStates = new Dictionary<string, Dictionary<string, string>>();
        foreach (var name in modules.Keys)
        {
            if (modules[name].type == "flip-flop")
            {
                flipFlopStates[name] = false;
            }
            else if (modules[name].type == "conjunction")
            {
                conjunctionStates[name] = modules.Keys.Where(n => modules[n].targets.Contains(name)).ToDictionary(n => n, n => "low");
            }
        }

        var lastNodeBeforeRx = modules.Keys.FirstOrDefault(n => modules[n].targets.Contains("rx"));
        var secondLastNodes = modules.Keys.Where(n => modules[n].targets.Contains(lastNodeBeforeRx)).ToList();
        var cycles = new Dictionary<string, long>();
        var i = 0L;
        while (cycles.Count < secondLastNodes.Count)
        {
            var signals = new Queue<(string source, string pulse, string target)>();
            signals.Enqueue(("button", "low", "broadcaster"));

            while (signals.Count > 0)
            {
                var signal = signals.Dequeue();

                if (modules.ContainsKey(signal.target))
                {
                    var target = modules[signal.target];
                    switch (target.type)
                    {
                        case "broadcaster":
                            foreach (var t in target.targets)
                            {
                                signals.Enqueue((signal.target, signal.pulse, t));
                            }
                            break;
                        case "flip-flop":
                            if (signal.pulse == "low")
                            {
                                var flipFlopState = !flipFlopStates[signal.target];
                                foreach (var t in target.targets)
                                {
                                    signals.Enqueue((signal.target, flipFlopState ? "high" : "low", t));
                                }
                                flipFlopStates[signal.target] = flipFlopState;
                            }
                            break;
                        case "conjunction":
                            var conjunctionState = conjunctionStates[signal.target];
                            conjunctionState[signal.source] = signal.pulse;
                            foreach (var t in target.targets)
                            {
                                signals.Enqueue((signal.target, conjunctionState.Values.All(v => v == "high") ? "low" : "high", t));
                            }
                            conjunctionStates[signal.target] = conjunctionState;
                            break;
                    }
                }
                if (signal.pulse == "high" && secondLastNodes.Contains(signal.source))
                {
                    if (!cycles.ContainsKey(signal.source))
                    {
                        cycles[signal.source] = i + 1;
                    }
                }
            }
            i++;
        }

        return cycles.Values.Aggregate(1L, (a, b) => a * b);
    }
}