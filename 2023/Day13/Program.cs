// Task https://adventofcode.com/2023/day/13
Console.WriteLine("AoC 2023 - Day 13");

var inputData = File.ReadAllText("inputValues.txt");

var patterns = inputData
    .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
    .Select(p => p.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()).ToList();

var firstPart =  patterns
    .Select(p => Calculate(p, 0))
    .Sum();

Console.WriteLine($"First part {firstPart}");

var secondPart = patterns
    .Select(p => Calculate(p, 1))
    .Sum();

Console.WriteLine($"Second part {secondPart}");
Console.ReadKey();

static int Calculate(char[][] matrix, int noOfSmudges) =>
    FindMirror(matrix, noOfSmudges) + 100 * FindMirror(Transpose(matrix), noOfSmudges);

static int FindMirror(char[][] pattern, int maxSmudges = 0)
{
    for (var mirrorPosition = 1; mirrorPosition < pattern[0].Length; mirrorPosition++)
    {
        var smudges = 0;
        var startIndex = Math.Max(0, mirrorPosition * 2 - pattern[0].Length);

        foreach (var line in pattern)
        {
            for (var x = startIndex; x < mirrorPosition; x++)
            {
                var leftElement = line[x];
                var rightElement = line[mirrorPosition * 2 - 1 - x];

                if (leftElement != rightElement) smudges++;
                if (SmudgesOverflow(maxSmudges, smudges)) break;
            }

            if (SmudgesOverflow(maxSmudges, smudges)) break;
        }

        if (smudges == maxSmudges) return mirrorPosition;
    }

    return 0;
}

static bool SmudgesOverflow(int max, int current) => current > max;

static char[][] Transpose(char[][] matrix) =>
    matrix[0].Select((col, i) => matrix.Select(row => row[i]).ToArray()).ToArray();