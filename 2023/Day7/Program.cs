// Task https://adventofcode.com/2023/day/7
Console.WriteLine("AoC 2023 - Day 7");

var inputData = File.ReadAllLines("inputValues.txt");

var cards = inputData.Select(l =>
    {
        var parts = l.Split(" ");
        return new Hand(parts[0], int.Parse(parts[1]));
    }).ToList();

cards.Sort();

int firstPart = cards.Select((card, i) => card.Bid * (i + 1)).Sum();
Console.WriteLine($"First part: {firstPart}");


var cards2 = inputData.Select(l =>
{
    var parts = l.Split(" ");
    return new Hand(parts[0], int.Parse(parts[1]), true);
}).ToList();

cards2.Sort();

int secondPart = cards2.Select((card, i) => card.Bid * (i + 1)).Sum();
Console.WriteLine($"Second part: {secondPart}");
Console.ReadLine();




internal class Hand : IComparable<Hand>
{
    bool _secondPart;

    public Hand(string cards, int bid, bool secondPart = false) 
    {
        if (cards.Length != 5) throw new ArgumentException("Invalid hand size.");

        _secondPart = secondPart;
        Cards = cards;
        Bid = bid;
        Type = secondPart ? GetHandTypeSecondPart(cards) : GetHandType(cards);
    }

    public string Cards { get; set; }
    public int Bid { get; set; }
    public HandType Type { get; set; }

    public int CompareTo(Hand? other)
    {
        if (Type > other!.Type)
            return 1;

        if (Type < other!.Type)
            return -1;

        for (int i = 0; i < 5; i++)
        {
            var cmpRes = CompareCards(Cards[i], other.Cards[i]);
            if (cmpRes != 0) return cmpRes;
        }
        return 0;
    }

    private int CompareCards(char card1, char card2)
    {
        string cardOrder = _secondPart ? "AKQT98765432J" : "AKQJT98765432";

        return cardOrder.IndexOf(card2) - cardOrder.IndexOf(card1);
    }

    private HandType GetHandType(string hand)
    {
        if (hand.Distinct().Count() == 1) 
            return HandType.FiveOfAKind;

        if (hand.GroupBy(c => c).FirstOrDefault(group => group.Count() == 4) is not null)
            return HandType.FourOfAKind;

        var groups = hand.GroupBy(c => c).Select(group => group.Count()).OrderByDescending(count => count).ToList();
        if (groups.Count == 2 && groups[0] == 3 && groups[1] == 2)
            return  HandType.FullHouse;

        if (hand.GroupBy(c => c).FirstOrDefault(group => group.Count() == 3) is not null)
            return HandType.ThreeOfAKind;

        if (hand.GroupBy(c => c).Where(group => group.Count() == 2).Count() == 2)
            return HandType.TwoPair;

        if (hand.GroupBy(c => c).Where(group => group.Count() == 2).Count() == 1)
            return HandType.OnePair;

        if (hand.Distinct().Count() == 5)
            return HandType.HighCard;

        throw new ArgumentException("Invalid hand.");
    }

    private HandType GetHandTypeSecondPart(string hand)
    {
        if (hand.Distinct().Count() == 1)
            return HandType.FiveOfAKind;

        var noOfJ = hand.Count(c => c == 'J');
        var handTemp = hand.Replace("J", "");

        if (handTemp.GroupBy(c => c).FirstOrDefault(group => group.Count() == 4) is not null)
            return IncreaseType(HandType.FourOfAKind, noOfJ);

        var groups = handTemp.GroupBy(c => c).Select(group => group.Count()).OrderByDescending(count => count).ToList();
        if (groups.Count == 2 && groups[0] == 3 && groups[1] == 2)
            return IncreaseType(HandType.FullHouse, noOfJ) ;

        if (handTemp.GroupBy(c => c).FirstOrDefault(group => group.Count() == 3) is not null)
            return IncreaseType(HandType.ThreeOfAKind, noOfJ);

        if (handTemp.GroupBy(c => c).Where(group => group.Count() == 2).Count() == 2)
            return IncreaseType(HandType.TwoPair, noOfJ);

        if (handTemp.GroupBy(c => c).Where(group => group.Count() == 2).Count() == 1)
            return IncreaseType(HandType.OnePair, noOfJ) ;

        return IncreaseType(HandType.HighCard, noOfJ);
    }

    private HandType IncreaseType(HandType old, int noOfJ)
    {
        if (noOfJ == 0) return old;
        noOfJ--;
        
        switch(old)
        {
            case HandType.HighCard: return IncreaseType(HandType.OnePair, noOfJ);
            case HandType.OnePair: return IncreaseType(HandType.ThreeOfAKind, noOfJ);
            case HandType.TwoPair: return IncreaseType(HandType.FullHouse, noOfJ);
            case HandType.ThreeOfAKind: return IncreaseType(HandType.FourOfAKind, noOfJ);
            case HandType.FullHouse: return IncreaseType(HandType.FourOfAKind, noOfJ);
            case HandType.FourOfAKind: return IncreaseType(HandType.FiveOfAKind, noOfJ);
            case HandType.FiveOfAKind: return IncreaseType(HandType.FiveOfAKind, noOfJ);
            default: return old;
        }
    }
}

internal enum HandType
{
    HighCard = 1,       //23456
    OnePair = 2,        //22345
    TwoPair = 3,        //22334
    ThreeOfAKind = 4,   //22234
    FullHouse = 5,      //22233
    FourOfAKind = 6,    //22223
    FiveOfAKind = 7,    //22222
}