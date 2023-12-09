// Task https://adventofcode.com/2023/day/9
Console.WriteLine("AoC 2023 - Day 9");

var inputData = File.ReadAllLines("inputValues1.txt");
var asd = Extrapolate(inputData[2].Split(' ').Select(int.Parse).ToList());

var firstPart = inputData
    .Select(line => line.Split(' ').Select(int.Parse))
    .Select(sequence => Extrapolate(sequence.ToList()))
    .Sum();

Console.WriteLine($"First part: {firstPart}");

var secondPart = inputData
    .Select(line => line.Split(' ').Select(int.Parse))
    .Select(sequence => ExtrapolateBackwards(sequence.ToList()))
    .Sum();

Console.WriteLine($"Second part: {secondPart}");
Console.ReadKey();

static int Extrapolate(List<int> sequence)
{
    if (sequence.All(n => n == 0))
        return 0;

    return Extrapolate(NextDifference(sequence)) + sequence[^1];
}

static int ExtrapolateBackwards(List<int> sequence)
{
    if (sequence.All(n => n == 0))
        return 0;

    return sequence[0] - ExtrapolateBackwards(NextDifference(sequence));
}

static List<int> NextDifference(List<int> sequence)
{
    return sequence.Skip(1).Select((x, i) => x - sequence[i]).ToList();
}