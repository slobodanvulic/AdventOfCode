// Task https://adventofcode.com/2023/day/14
Console.WriteLine("AoC 2023 - Day 14");

var inputData = File.ReadAllLines("inputValues.txt");

var stones = inputData.Select(line => line.ToCharArray()).ToArray();

RollNorth(stones);
var firstPart = CalculateLoad(stones);
Console.WriteLine($"First part {firstPart}");

// Second part
stones = inputData.Select(line => line.ToCharArray()).ToArray(); //reset stone map
var noOfCycles = 1_000_000_000;

var stoneCombinations = new HashSet<int>();
var loads = new List<int>();
var firstCycleEnd = 0;
var secondCycleEnd = 0;
var firstCycle = true;

for (var i = 0; i <= noOfCycles-1; i++)
{
    for (var j = 1; j <= 4; j++)
    {
        RollNorth(stones);
        stones = RotateMatrixClockwise(stones);
    }
    if(firstCycle && !stoneCombinations.Add(GetMatrixHashCode(stones)))
    {
        firstCycleEnd = i;
        firstCycle = false;
        stoneCombinations.Clear();
    }
    if (!firstCycle && !stoneCombinations.Add(GetMatrixHashCode(stones)))
    {
        secondCycleEnd = i;
        loads.Add(CalculateLoad(stones));
        break;
    }

    loads.Add(CalculateLoad(stones));
}

var cycle = loads.Skip(firstCycleEnd + 1).ToList();
var secondPart = cycle[(noOfCycles - 1 - secondCycleEnd - 1) % cycle.Count];
Console.WriteLine($"Second part {secondPart}");
Console.ReadKey();

static void RollNorth(char[][] chars)
{
    var moved = false;

    while (true)
    {
        for (var i = chars.Length - 2; i >= 0; i--)
        {
            var upLine = chars[i];
            var downLine = chars[i + 1];

            for (var j = 0; j < chars[0].Length; j++)
            {
                if (upLine[j] == '#') continue;
                if (upLine[j] == '.' && downLine[j] == 'O')
                {
                    upLine[j] = 'O';
                    downLine[j] = '.';
                    moved = true;
                }
            }
        }

        if (!moved) break;
        moved = false;
    }
}

static char[][] RotateMatrixClockwise(char[][] matrix)
{
    var rows = matrix.Length;
    var cols = matrix[0].Length;
    var rotatedMatrix = new char[cols][];

    for (var i = 0; i < cols; i++)
    {
        rotatedMatrix[i] = new char[rows];
        for (var j = 0; j < rows; j++)
        {
            rotatedMatrix[i][j] = matrix[rows - 1 - j][i];
        }
    }

    return rotatedMatrix;
}


static int CalculateLoad(char[][] matrix) =>
    matrix.Select((row, i) => row.Count(ch => ch == 'O') * (matrix.Length - i)).Sum();

static int GetMatrixHashCode(char[][] matrix) => 
    matrix.SelectMany(row => row).Aggregate(17, (first, second) => first * 23 + second.GetHashCode());