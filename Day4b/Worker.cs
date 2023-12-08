namespace AdventOfCode2023.Day4b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var cards = new List<Card>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(new[] { ':', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var card = new Card(
                lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList(),
                lineParts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList());
            cards.Add(card);
        }

        for (var cardIndex = 0; cardIndex < cards.Count; cardIndex++)
        {
            var card = cards[cardIndex];
            var winners = card.winningNumbers.Intersect(card.numbersYouHave).ToList();
            for (var copyIndex = 0; copyIndex < winners.Count; copyIndex++)
            {
                var copyCard = cards[cardIndex + copyIndex + 1];
                copyCard.numberOfCard += card.numberOfCard;
            }
        }
        return cards.Sum(c => c.numberOfCard);
    }
}

record Card
{
    public List<int> winningNumbers;
    public List<int> numbersYouHave;
    public int numberOfCard;

    public Card(List<int> winningNumbers, List<int> numbersYouHave)
    {
        this.winningNumbers = winningNumbers;
        this.numbersYouHave = numbersYouHave;
        this.numberOfCard = 1;
    }
}
