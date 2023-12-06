// Task https://adventofcode.com/2023/day/2

using System.Text.RegularExpressions;

Console.WriteLine("AoC 2023 - Day 2");

var inputValues = File.ReadAllLines("inputValues.txt"); ;

var res1 = SolveFirstPart(inputValues);
var res2 = SolveSecondPart(inputValues);

Console.WriteLine($"Part 1: {res1}");
Console.WriteLine($"Part 2: {res2}");
Console.ReadKey();

static string SolveFirstPart(string[] input) =>
    input
        .Select((line, i) => new Game(
            i + 1,
            new Regex(@"\d+ blue").Matches(line)
               .Select(m => int.Parse(m.Value.Split(" ")[0]))
               .Max(),
            new Regex(@"\d+ green").Matches(line)
                .Select(m => int.Parse(m.Value.Split(" ")[0]))
                .Max(),
            new Regex(@"\d+ red").Matches(line)
                .Select(m => int.Parse(m.Value.Split(" ")[0]))
                .Max()))
        .Where(g => g.MaxBlue <= 14 && g.MaxGreen <= 13 && g.MaxRed <= 12)
        .Sum(g => g.GameId)
        .ToString();

static string SolveSecondPart(string[] input) =>
    input
        .Select((line, i) =>  new Game(
            i+1,
            new Regex(@"\d+ blue").Matches(line)
               .Select(m => int.Parse(m.Value.Split(" ")[0]))
               .Max(),
            new Regex(@"\d+ green").Matches(line)
                .Select(m => int.Parse(m.Value.Split(" ")[0]))
                .Max(),
            new Regex(@"\d+ red").Matches(line)
                .Select(m => int.Parse(m.Value.Split(" ")[0]))
                .Max()))
        .Select(g => g.MaxBlue * g.MaxGreen * g.MaxRed)
        .Sum()
        .ToString();


internal record Game(int GameId, int MaxBlue, int MaxGreen, int MaxRed);