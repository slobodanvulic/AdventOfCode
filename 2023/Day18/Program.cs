// Task https://adventofcode.com/2023/day/18
using NetTopologySuite.Geometries;

Console.WriteLine("AoC 2023 - Day 18");

var inputData = File.ReadAllLines("inputValues.txt");
var instructions = inputData.Select(line => line.Split()).Select(line => new Instruction(line[0], int.Parse(line[1]), line[2]));

var firstPart = Area(instructions);
Console.WriteLine($"First part {firstPart}");


var secondPartInstructions = instructions
    .Select(i => i.ColorCode)
    .Select(c => new Instruction(ToDirection(c.Substring(7, 1)), int.Parse(c.Substring(2, 5), System.Globalization.NumberStyles.HexNumber), ""));

var secondPart = Area(secondPartInstructions);
Console.WriteLine($"Second part {secondPart}");

Console.ReadKey();


static double Area(IEnumerable<Instruction> instructions)
{
    // Calculation should be done using Shoelace formula
    // Polygon.Area (from NetTopologySuite.Geometries) uses Shoelace to calculate area

    var coordinates = new List<Coordinate>() { new Coordinate(0, 0) };
    var current = new Location(0, 0);

    foreach (var instruction in instructions)
    {
        current = current.Move(instruction.Direction, instruction.Steps);
        coordinates.Add(new Coordinate(current.X, current.Y));
    }

    var geometryFactory = new GeometryFactory();
    var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());

    // Pick's theorem
    // interior = area - boyndary / 2 + 1
    // result = interior + boundary

    return polygon.Area + polygon.Boundary.Length / 2 + 1;
}

static string ToDirection(string number) =>
    number switch
    {
        "0" => "R",
        "1" => "D",
        "2" => "L",
        "3" => "U",
        _ => throw new ArgumentException()
    };

record Instruction(string Direction, int Steps, string ColorCode);

record struct Location(int X, int Y)
{
    public Location Move(string direction, int steps) =>
        direction switch
        {
            "U" => new Location(X, Y + steps),
            "D" => new Location(X, Y - steps),
            "L" => new Location(X - steps, Y),
            "R" => new Location(X + steps, Y),
            _ => this
        };
}