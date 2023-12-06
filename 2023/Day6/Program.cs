// Task https://adventofcode.com/2023/day/6
Console.WriteLine("AoC 2023 - Day 6");

var inputData = File.ReadAllLines("inputValues.txt");

//First part
var times = (inputData[0].Split(":", StringSplitOptions.TrimEntries))[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
var distances = (inputData[1].Split(":", StringSplitOptions.TrimEntries))[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

var races = times.Zip(distances, (t, d) => new Race() { Time = int.Parse(t), Distance = int.Parse(d) });
var noWays = races.Select(r => r.WaysToWin()).Aggregate((first,second) => first * second);

Console.WriteLine($"First part: {noWays}");

//Second part
var time = long.Parse((inputData[0].Split(":", StringSplitOptions.TrimEntries))[1].Replace(" ", ""));
var distance = long.Parse((inputData[1].Split(":", StringSplitOptions.TrimEntries))[1].Replace(" ", ""));
long noWays2 = 0;
for(long i = 0; i < time; i++)
{
    if (i * (time - i) > distance) noWays2++;
}

Console.WriteLine($"Second part: {noWays2}");
Console.ReadLine();


internal class Race
{
    public int Time { get; set; }
    public int  Distance { get; set; }
    public int WaysToWin() => Enumerable.Range(0, Time).Count(x => x * (Time - x) > Distance);
}