using System.Dynamic;

namespace AdventOfCode2023.Day4a;

public class Worker : IWorker
{
    public int DoWork(string inputFile)
    {
        var cards = new List<Card>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var card = new Card();
            var lineParts = line.Split(new[] { ':', '|' }, StringSplitOptions.RemoveEmptyEntries);
            card.winningNumbers = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
            card.numbersYouHave = lineParts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
            cards.Add(card);
        }

        var sum = 0;
        foreach (var card in cards)
        {
            var winners = card.winningNumbers.Intersect(card.numbersYouHave).ToList();
            if (winners.Any())
            {
                sum += Enumerable.Repeat(2, winners.Count() - 1).Aggregate(1, (a, b) => a * b);
            }
        }
        return sum;
    }
}

struct Card
{
    public List<int> winningNumbers;
    public List<int> numbersYouHave;
}
