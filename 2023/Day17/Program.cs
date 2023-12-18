// Task https://adventofcode.com/2023/day/17
using Queue = System.Collections.Generic.PriorityQueue<(Location Location, Direction Direction, int Lenght), int>;

Console.WriteLine("AoC 2023 - Day 17");

var inputData = File.ReadAllLines("inputValues.txt");
var map = inputData.Select(line => line.ToCharArray().Select(ch => int.Parse(ch.ToString())).ToArray()).ToArray();

var start = new Location(0, 0);
var finish = new Location(map[0].Length - 1, map.Length - 1);

var priorityQueue = new Queue();
var alreadyVisited = new HashSet<(Location Location, Direction Direction, int Lenght)>();
priorityQueue.Enqueue((start, Direction.Right, 0), 0);
priorityQueue.Enqueue((start, Direction.Down, 0), 0);

var firstPart = CalculateHeatLost(priorityQueue, map, finish, alreadyVisited, 0, 3);
Console.WriteLine($"First part {firstPart}");


priorityQueue.Clear();
alreadyVisited.Clear();
priorityQueue.Enqueue((start, Direction.Right, 0), 0);
priorityQueue.Enqueue((start, Direction.Down, 0), 0);

var secondPart = CalculateHeatLost(priorityQueue, map, finish, alreadyVisited, 4, 10);
Console.WriteLine($"Second part {secondPart}");

Console.ReadKey();

static int CalculateHeatLost(Queue priorityQueue, int[][] map, Location finish, HashSet<(Location Location, Direction Direction, int Lenght)> visited, int minLen, int maxLen)
{
    while (priorityQueue.TryDequeue(out var item, out int heatLoss))
    {
        if (item.Location == finish) return heatLoss;

        foreach(var nextItem in PossibleMoves(map, item, maxLen, minLen)) {
            if (visited.Add(nextItem))
            {
                priorityQueue.Enqueue(nextItem, heatLoss + GetFromMap(map, nextItem.Location));
            }
        }
    }

    return map[0][0];
}

static IEnumerable<(Location Location, Direction Direction, int Lenght)> PossibleMoves(int[][] map, (Location Location, Direction Direction, int Lenght) item, int maxLength, int minLenght = 0)
{
    if (item.Lenght < maxLength 
        && ContainsInMap(map, item.Location.Move(item.Direction)))
        yield return (item.Location.Move(item.Direction), item.Direction, item.Lenght + 1);

    if(item.Lenght >= minLenght)
    {
        switch (item.Direction)
        {
            case Direction.Right:
            case Direction.Left:
                {
                    var canUp = ContainsInMap(map, item.Location.Move(Direction.Up));
                    var canDown = ContainsInMap(map, item.Location.Move(Direction.Down));
                    if (canUp) yield return (item.Location.Move(Direction.Up), Direction.Up, 1);
                    if (canDown) yield return (item.Location.Move(Direction.Down), Direction.Down, 1);
                    break;
                }
            case Direction.Up:
            case Direction.Down:
                {
                    var canLeft = ContainsInMap(map, item.Location.Move(Direction.Left));
                    var canRight = ContainsInMap(map, item.Location.Move(Direction.Right));
                    if (canLeft) yield return (item.Location.Move(Direction.Left), Direction.Left, 1);
                    if (canRight) yield return (item.Location.Move(Direction.Right), Direction.Right, 1);
                    break;
                }
        }
    } 
}

static bool ContainsInMap(int[][] map, Location location) =>
    map.Length > location.Y
        && location.Y >= 0
        && map[0].Length > location.X
        &&location.X >= 0;

static int GetFromMap(int[][] map, Location location) =>
    map[location.Y][location.X];

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
