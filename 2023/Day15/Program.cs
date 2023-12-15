// Task https://adventofcode.com/2023/day/15
Console.WriteLine("AoC 2023 - Day 15");

var inputData = File.ReadAllText("inputValues.txt");

var firstPart = inputData.Split(',', StringSplitOptions.TrimEntries).Select(Hash).Sum();
Console.WriteLine($"First part {firstPart}");


var regexDash = @".+-$";
var boxes = new List<List<Lens>>(256);
boxes.AddRange(Enumerable.Range(0, 256).Select(i => new List<Lens>()));


foreach (var instruction in inputData.Split(',', StringSplitOptions.TrimEntries))
{
    if (instruction.Contains('='))
    {
        var parts = instruction.Split('=');
        var lens = new Lens() {Label = parts[0], FocalLength = int.Parse(parts[1])};
        var hash = Hash(lens.Label);
        var index = boxes[hash].FindIndex(l => l.Label == lens.Label);
        if (index >= 0)
        {
            boxes[hash][index].FocalLength = lens.FocalLength;
        }
        else
        {
            boxes[hash].Add(lens);
        }
    }
    else
    {
        var lensToRemove = instruction.Substring(0, instruction.Length - 1);
        var hash = Hash(lensToRemove);
        var index = boxes[hash].FindIndex(l => l.Label == lensToRemove);
        if (index >= 0)
        {
            boxes[hash].RemoveAt(index);
        }
    }
}



var secondPart = boxes.Select((box, i) => box.Select((lens, j) => (i + 1) * (j + 1) * lens.FocalLength).Sum()).Sum();
Console.WriteLine($"second part {secondPart}");
Console.ReadKey();
static int Hash(string input)
{
    var res = 0;

    foreach (var ch in input)
    {
        res += ch;
        res *= 17;
        res %= 256;
    }

    return res;
}

internal class Lens
{
    public string Label { get; set; }
    public int FocalLength { get; set; }
}