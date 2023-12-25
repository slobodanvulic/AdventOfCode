// Task https://adventofcode.com/2023/day/21
Console.WriteLine("AoC 2023 - Day 21");

var inputData = File.ReadAllLines("inputValues.txt");

var possibleLocations = inputData
    .Select((line, y) =>
        Enumerable.Range(0, line.Length).Where(x => line[x] != '#').Select(x => new Location(x, y)))
    .SelectMany(l => l)
    .ToHashSet();

var startLocation = inputData
    .Select((line, y) =>
        Enumerable.Range(0, line.Length).Where(x => line[x] == 'S').Select(x => new Location(x, y)))
    .SelectMany(l => l)
    .First();

var visitedLocations = new HashSet<Location>() {startLocation};

var directions = new List<Direction>() { Direction.Up, Direction.Right, Direction.Left, Direction.Down, Direction.Left };

for (var i = 0; i < 64; i++)
{
    visitedLocations = VisitedLocations(possibleLocations, visitedLocations, directions);
}

Console.WriteLine($"First part {visitedLocations.Count}");
Console.ReadLine();

static HashSet<Location> VisitedLocations(
    HashSet<Location> possibleLocations, 
    HashSet<Location> visitedLocations,
    List<Direction> directions) =>
    visitedLocations
        .SelectMany(location => directions, (location, direction) =>
            location.Move(direction)).Where(possibleLocations.Contains).ToHashSet();


record struct Location(int X, int Y)
{
    public Location Move(Direction direction) =>
        direction switch
        {
            Direction.Up => new Location(X, Y - 1),
            Direction.Down => new Location(X, Y + 1),
            Direction.Left => new Location(X - 1, Y),
            Direction.Right => new Location(X + 1, Y),
            _ => throw new InvalidOperationException()
        };
}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}