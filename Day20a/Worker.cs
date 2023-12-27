namespace AdventOfCode2023.Day20a;

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

        var sumLow = 0L;
        var sumHigh = 0L;
        for (var i = 0; i < 1000; i++)
        {
            var signals = new Queue<(string source, string pulse, string target)>();
            signals.Enqueue(("button", "low", "broadcaster"));

            var low = 0L;
            var high = 0L;
            while (signals.Count > 0)
            {
                var signal = signals.Dequeue();

                if (signal.pulse == "low")
                {
                    low++;
                }
                else
                {
                    high++;
                }

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
            }
            sumLow += low;
            sumHigh += high;
        }

        var sum = sumLow * sumHigh;
        return sum;
    }
}