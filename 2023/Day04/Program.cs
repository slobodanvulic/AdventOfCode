// Task https://adventofcode.com/2023/day/4
Console.WriteLine("AoC 2023 - Day 4");

var res1 = SolveFirstPart("inputValues.txt");
Console.WriteLine($"First part {res1}");

var res2 = SolveSecondPart("inputValues.txt");
Console.WriteLine($"First part {res2}");
Console.ReadKey();

static int SolveFirstPart(string filePath)
{
    var lines = File.ReadLines(filePath);

    return lines
        .Select(l => ReadCard(l))
        .Sum(c => c.Score);
}

static int SolveSecondPart(string filePath)
{
    var lines = File.ReadLines(filePath);

    var cards = lines.Select(l => ReadCard(l)).ToList();

    var counts = cards.Select(_ => 1).ToArray();

    for (var i = 0; i < cards.Count; i++)
    {
        var (card, count) = (cards[i], counts[i]);
        for (var j = 0; j < card.Matches; j++)
        {
            counts[i + j + 1] += count;
        }
    }
    return counts.Sum();
}


static Card ReadCard(string line)
{
    var temp1 = line.Split(':');
    var temp2 = temp1[1].Split("|");

    var cardId = int.Parse(temp1[0].Substring(5));

    var winningNumbers = ParseNumbers(temp2[0].Trim().Split(" ")).ToList();
    var numbers = ParseNumbers(temp2[1].Trim().Split(" ")).ToList();

    int score = 0;
    int matches = 0;

    foreach(var num in winningNumbers)
    {
        if (numbers.Contains(num))
        {
            matches++;
            score = score == 0 ? 1 : score * 2;
        }
    }

    return new Card(cardId,winningNumbers, numbers, score, matches);
 }

static IEnumerable<int> ParseNumbers(string[] nums)=>
    nums
        .Where(num => num.Trim().Length > 0)
        .Select(int.Parse);


internal record Card(int Id, List<int> WinningNumbers, List<int> Numbers, int Score, int Matches);