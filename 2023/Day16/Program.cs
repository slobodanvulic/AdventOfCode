// Task https://adventofcode.com/2023/day/16
Console.WriteLine("AoC 2023 - Day 16");

var inputData = File.ReadAllLines("inputValues.txt");
var map = inputData.Select(line => line.ToCharArray()).ToArray();

int firstPart = EnergizeMap(map, new Location(0, 0), Direction.Right);
Console.WriteLine($"First part {firstPart}");


var energizedCombinations = new List<int>();

for(int y = 0; y < map.Length; y++)
{
    energizedCombinations.Add(EnergizeMap(map, new Location(0, y), Direction.Right));
    energizedCombinations.Add(EnergizeMap(map, new Location(map[0].Length -1 , y), Direction.Left));
}

for (int x = 0; x < map.Length; x++)
{
    energizedCombinations.Add(EnergizeMap(map, new Location(x, 0), Direction.Down));
    energizedCombinations.Add(EnergizeMap(map, new Location(x, map.Length -1), Direction.Up));
}

Console.WriteLine($"Second part {energizedCombinations.Max()}");
Console.ReadKey();


static int EnergizeMap(char[][] map, Location startPosition, Direction direction)
{
    var energizedTiles = new HashSet<Location>();
    var alreadyVisited = new HashSet<(Location, Direction)>();
    var start = new Beam(startPosition, direction);
    var beams = new List<Beam>() { start }; 

    do
    {
        for (int i = beams.Count - 1; i >= 0; i--)
        {
            energizedTiles.Add(beams[i].CurrentLocation);
            Move(beams[i], map, beams);
            if (beams[i].End || !alreadyVisited.Add(new(beams[i].CurrentLocation, beams[i].CurrentDirection)))
            {
                beams.Remove(beams[i]);
            }

        }
    } while (beams.Count > 0);

    return energizedTiles.Count;
}

static void Move(Beam beam, char[][] map, List<Beam> beams)
{
    var width = map[0].Length;
    var height = map.Length;
    switch (map[beam.CurrentLocation.Y][beam.CurrentLocation.X], beam.CurrentDirection)
    {
        case ('.', Direction.Right):
        case ('-', Direction.Right):
        case ('/', Direction.Up):
        case ('\\', Direction.Down): // Right
            {
                if (beam.CurrentLocation.X + 1 >= width) beam.End = true;
                beam.CurrentLocation = new Location(beam.CurrentLocation.X + 1, beam.CurrentLocation.Y);
                beam.CurrentDirection = Direction.Right;
                break;
            }
        case ('.', Direction.Left):
        case ('-', Direction.Left):
        case ('/', Direction.Down):
        case ('\\', Direction.Up): // Left
            {
                if (beam.CurrentLocation.X - 1 < 0) beam.End = true;
                beam.CurrentLocation = new Location(beam.CurrentLocation.X - 1, beam.CurrentLocation.Y);
                beam.CurrentDirection = Direction.Left;
                break;
            }
        case ('.', Direction.Up):
        case ('|', Direction.Up):
        case ('/', Direction.Right):
        case ('\\', Direction.Left): // Up
            {
                if (beam.CurrentLocation.Y - 1 < 0) beam.End = true;
                beam.CurrentLocation = new Location(beam.CurrentLocation.X, beam.CurrentLocation.Y -1);
                beam.CurrentDirection = Direction.Up;
                break;
            }
        case ('.', Direction.Down):
        case ('|', Direction.Down):
        case ('/', Direction.Left):
        case ('\\', Direction.Right): // Down
            {
                if (beam.CurrentLocation.Y + 1 >= height) beam.End = true;
                beam.CurrentLocation = new Location(beam.CurrentLocation.X, beam.CurrentLocation.Y + 1);
                beam.CurrentDirection = Direction.Down;
                break;
            }
        case ('|', Direction.Left):
        case ('|', Direction.Right): // split up and down
            {
                var currentY = beam.CurrentLocation.Y;
                if (currentY + 1 >= height) beam.End = true;
                beam.CurrentLocation = new Location(beam.CurrentLocation.X, currentY + 1);
                beam.CurrentDirection = Direction.Down;

                if (currentY - 1  >= 0)
                    beams.Add(new Beam(new Location(beam.CurrentLocation.X, currentY - 1), Direction.Up));
                break;
            }
        case ('-', Direction.Up):
        case ('-', Direction.Down): // split left and right
            {
                var currentX = beam.CurrentLocation.X;
                if (currentX + 1 >= width) beam.End = true;
                beam.CurrentLocation = new Location(currentX + 1, beam.CurrentLocation.Y);
                beam.CurrentDirection = Direction.Right;

                if (currentX - 1 >= 0)
                    beams.Add(new Beam(new Location(currentX - 1, beam.CurrentLocation.Y), Direction.Left));
                break;
            }
    }
}


internal class Beam
{
    public Beam(Location location, Direction direction)
    {
        CurrentDirection= direction;
        CurrentLocation= location;
        End = false;
    }
    public bool End { get; set; }
    public Location CurrentLocation { get; set; }
    public Direction CurrentDirection { get; set; }
}

record struct Location(int X, int Y);

enum Direction
{
    Up,
    Right,
    Down,
    Left
}