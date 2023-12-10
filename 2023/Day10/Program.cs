// Task https://adventofcode.com/2023/day/10

using NetTopologySuite.Geometries;

Console.WriteLine("AoC 2023 - Day 10");

var inputData = File.ReadAllLines("inputValues.txt");

var position = StartPosition(inputData);
var direction = StartDirection(inputData, position);
var cordinates = new List<Coordinate>() { new Coordinate(position.x, position.y)};
int steps = 0;
var current = 'S';

while (true)
{
    switch (direction)
    {
        case Direction.Up:
            position.y--;
            current = inputData[position.y][position.x];
            if (current == '7') direction = Direction.Left;
            else if (current == 'F') direction = Direction.Right;
            break;
        case Direction.Down:
            position.y++;
            current = inputData[position.y][position.x];
            if (current == 'J') direction = Direction.Left;
            else if (current == 'L') direction = Direction.Right;
            break;
        case Direction.Left:
            position.x--;
            current = inputData[position.y][position.x];
            if (current == 'F') direction = Direction.Down;
            else if (current == 'L') direction = Direction.Up;
            break;
        case Direction.Right:
            position.x++;
            current = inputData[position.y][position.x];
            if (current == 'J') direction = Direction.Up;
            else if (current == '7') direction = Direction.Down;
            break;
    }
    steps++;
    cordinates.Add(new Coordinate(position.x, position.y));
    if (current == 'S') break;
}

var firstPart = steps / 2;
Console.WriteLine($"First part {firstPart}");



var geometryFactory = new GeometryFactory();
var polygon = geometryFactory.CreatePolygon(cordinates.ToArray());
var seconfPart = 0;

for (int i = 0; i < inputData.Length; i++)
    for (int j = 0; j < inputData[i].Length; j++)
        if (polygon.Contains(geometryFactory.CreatePoint(new Coordinate(j, i)))) 
            seconfPart++;
    

Console.WriteLine($"Second part {seconfPart}");
Console.ReadKey();

static (int x, int y) StartPosition(string[] input)
{
    var ret = (-1, -1);

    for (int i = 0; i < input.Length; i++)
        for (int j = 0; j < input[i].Length; j++)
            if (input[i][j] == 'S') ret = (j, i);

    return ret;
}

static Direction StartDirection(string[] input, (int x, int y) start)
{
    Direction direction = Direction.Up;

    if (CanRight(input, start.x, start.y) && "-7J".Contains(input[start.y][start.x + 1])) direction = Direction.Right;
    else if (CanDown(input, start.y) && "|JL".Contains(input[start.y - 1][start.x])) direction = Direction.Down;
    else if (CanLeft(start.x) && "L-F".Contains(input[start.y][start.x - 1])) direction = Direction.Left;

    return direction;
}

static bool CanRight(string[] input, int x, int y) => input[y].Length > (x + 1);
static bool CanDown(string[] input, int y) => input.Length < (y + 1);
static bool CanLeft(int x) => x > 0;

internal enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
