namespace AdventOfCode2023.Day7b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var hands = new List<Hand>();

        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(" ");
            var hand = new Hand { cards = lineParts[0].ToCharArray(), bid = int.Parse(lineParts[1]) };
            hands.Add(hand);
        }

        hands.Sort((a, b) =>
        {
            var handTypeA = CalculateHandType(a.cards);
            var handTypeB = CalculateHandType(b.cards);
            var result = handTypeA.CompareTo(handTypeB);

            var cardIndex = 0;
            while (result == 0)
            {
                var rankA = GetRank(a.cards[cardIndex]);
                var rankB = GetRank(b.cards[cardIndex]);
                result = rankA.CompareTo(rankB);
                cardIndex++;
            }
            return result;
        });

        var sum = 0L;
        for (var i = 0; i < hands.Count; i++)
        {
            sum += (i + 1) * hands[i].bid;
        }
        return sum;
    }

    private HandTypes CalculateHandType(char[] hand)
    {
        var distinctCards = hand.Where(c => c != 'J').Distinct();
        var countOfJokerCards = hand.Where(c => c == 'J').Count();
        var countOfFirstCard = distinctCards.Any() ? hand.Where(c => c == distinctCards.First()).Count() : 0;
        switch (distinctCards.Count())
        {
            case 0:
            case 1:
                return HandTypes.FiveOfAKind;

            case 2:
                if (countOfFirstCard == 1 || countOfFirstCard + countOfJokerCards == 4)
                {
                    return HandTypes.FourOfAKind;
                }
                else
                {
                    return HandTypes.FullHouse;
                }

            case 3:
                var countOfLastCard = hand.Where(c => c == distinctCards.Last()).Count();
                if (countOfFirstCard + countOfJokerCards == 3 || countOfLastCard + countOfJokerCards == 3 || countOfFirstCard == 1 && countOfLastCard == 1)
                {
                    return HandTypes.ThreeOfAKind;
                }
                else
                {
                    return HandTypes.TwoPair;
                }

            case 4:
                return HandTypes.OnePair;

            default:
                return HandTypes.HighCard;
        }
    }

    private int GetRank(char card)
    {
        return card switch
        {
            'T' => 10,
            'J' => 1,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => card - '0',
        };
    }
}

struct Hand
{
    public char[] cards;
    public int bid;

    public override string ToString()
    {
        return new string(cards) + " " + bid;
    }
}

enum HandTypes
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}