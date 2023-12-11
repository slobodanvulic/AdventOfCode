// Task https://adventofcode.com/2023/day/11
Console.WriteLine("AoC 2023 - Day 11");


var inputData = File.ReadAllLines("inputValues.txt");

var galaxyCoordinates = inputData.SelectMany((line, y) =>line.Select((ch, x) => (x, y, ch)).Where(i => i.ch == '#')).ToList();
var emptyRows = Enumerable.Range(0, inputData.Length).Where(i => inputData[i].All(ch => ch == '.')).ToHashSet();
var emptyColumns = Enumerable.Range(0, inputData[0].Length).Where(i => inputData.All(st => st[i] == '.')).ToHashSet();

var cartesian = from galaxy1 in galaxyCoordinates
                from galaxy2 in galaxyCoordinates
                select new { galaxy1, galaxy2 };

var firstPart = cartesian
    .Select(x => Distance((x.galaxy1.x, x.galaxy1.y), (x.galaxy2.x, x.galaxy2.y), emptyRows, emptyColumns, 2))
    .Sum() / 2;

Console.WriteLine($"First part {firstPart}");

var secondPart = cartesian
    .Select(x => Distance((x.galaxy1.x, x.galaxy1.y), (x.galaxy2.x, x.galaxy2.y), emptyRows, emptyColumns, 1_000_000))
    .Sum() / 2;

Console.WriteLine($"Second part {secondPart}");
Console.ReadKey();


static long Distance((int x ,int y) a , (int x, int y) b, HashSet<int> emptyRows, HashSet<int> emptyCols, int expansion)
{
    var distanceX = Math.Abs(a.x - b.x);
    var distanceY = Math.Abs(a.y - b.y);

    distanceX = distanceX + (expansion - 1) * Enumerable.Range(Math.Min(a.x, b.x), distanceX).Count(emptyCols.Contains);
    distanceY = distanceY + (expansion - 1) * Enumerable.Range(Math.Min(a.y, b.y), distanceY).Count(emptyRows.Contains);
    
    return distanceX + distanceY;
}

