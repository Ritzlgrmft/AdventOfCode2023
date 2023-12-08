namespace AdventOfCode2023.Day2b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var games = new List<Game>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(":");

            var game = new Game
            {
                number = int.Parse(lineParts[0].Split(new[] { ' ', ':' })[1]),
                sets = new List<Set>()
            };

            var setParts = lineParts[1].Split(";");
            foreach (var setPart in setParts)
            {
                var set = new Set();
                var cubeParts = setPart.Split(",");
                foreach (var cubePart in cubeParts)
                {
                    var colorParts = cubePart.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    switch (colorParts[1])
                    {
                        case "red":
                            set.red = int.Parse(colorParts[0]);
                            break;
                        case "green":
                            set.green = int.Parse(colorParts[0]);
                            break;
                        case "blue":
                            set.blue = int.Parse(colorParts[0]);
                            break;
                    }
                }
                game.sets.Add(set);
            }
            games.Add(game);
        }

        var sum = 0;
        foreach (var game in games)
        {
            var redCount = game.sets.Max(s => s.red);
            var greenCount = game.sets.Max(s => s.green);
            var blueCount = game.sets.Max(s => s.blue);
            var power = redCount * greenCount * blueCount;
            sum += power;
        }
        return sum;
    }
}

struct Set
{
    public int red;
    public int green;
    public int blue;
}

struct Game
{
    public int number;
    public List<Set> sets;
}