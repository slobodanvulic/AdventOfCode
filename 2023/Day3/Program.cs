// Task https://adventofcode.com/2023/day/3
Console.WriteLine("AoC 2023 - Day 3");

var input = File.ReadAllLines("inputValues.txt");

var numbers = new List<Number>();
var symbols = new List<Symbol>();

for (var row = 0; row < input.Length; row++)
{
    var currentNumber = new Number();
    var digits = new List<int>();

    for (var col = 0; col < input[row].Length; col++)
    {
        if (input[row][col] == '.') continue;

        if (int.TryParse(input[row][col].ToString(), out var digit))
        {
            digits.Add(digit);
            if (digits.Count == 1)
            {
                currentNumber.StartX = col;
                currentNumber.StartY = row;
            }

            while (col < input[row].Length - 1 && int.TryParse(input[row][col + 1].ToString(), out digit))
            {
                digits.Add(digit);
                col++;
            }

            currentNumber.EndX = col;
            currentNumber.EndY = row;
            currentNumber.Value = int.Parse(string.Join("", digits));
            numbers.Add(currentNumber);
            currentNumber = new Number();
            digits.Clear();
        }
        else
        {
            symbols.Add(new Symbol
            {
                Value = input[row][col],
                X = col, Y = row,
            }); 
        }
    }
}

var part1 = numbers
    .Where(number => symbols.Any(number.IsAdjacent))
    .Sum(number => number.Value);

var part2 = symbols
    .Where(symbol => symbol.Value == '*')
    .Select(symbol => numbers.Where(number => number.IsAdjacent(symbol)).ToArray())
    .Where(gears => gears.Length == 2)
    .Sum(gears => gears[0].Value * gears[1].Value);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
Console.ReadKey();


internal struct Symbol
{
    public char Value { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

internal struct Number
{
    public int Value { get; set; }
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int EndX { get; set; }
    public int EndY { get; set; }

    public bool IsAdjacent(Symbol symbol) =>
        Math.Abs(symbol.Y - StartY) <= 1
           && symbol.X >= StartX - 1
           && symbol.X <= EndX + 1;
}
