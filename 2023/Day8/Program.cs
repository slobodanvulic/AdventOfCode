// Task https://adventofcode.com/2023/day/8
Console.WriteLine("AoC 2023 - Day 8");

var inputData = File.ReadAllLines("inputValues.txt");

var directions = inputData[0].Select(ch => ch == 'L' ? 0 : 1).ToList();
var map = inputData.Skip(2).ToDictionary(line => line.Substring(0, 3), line => line.Substring(7, 8).Split(",", StringSplitOptions.TrimEntries));

// first part
var firstPart = 0;
var index = 0;
var current = "AAA";
var finish = "ZZZ";

while (true)
{
    current = map[current][directions[index]];
    firstPart++;
    if (current == finish) break;
    index = index < (directions.Count - 1) ? index + 1 : 0;
}

Console.WriteLine($"First part: {firstPart}");

// second part
var currentNodes = map.Keys.Where(k => k.EndsWith('A')).ToList();
var steps = new List<long>(currentNodes.Count());

foreach(var node in currentNodes)
{
    current = node;
    index = 0;
    long counter = 0;

    while (true)
    {
        current = map[current][directions[index]];
        counter++;
        if (current[2] == 'Z') break;
        index = index < (directions.Count - 1) ? index + 1 : 0;
    }
    steps.Add(counter);
}


Console.WriteLine($"Second part: {steps.Aggregate((x, y) => LeastCommonMultiple(x, y))}");
Console.ReadKey();

static long GreatestCommonFactor (long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long LeastCommonMultiple(long a, long b)
{
    return (a / GreatestCommonFactor(a, b)) * b;
}

