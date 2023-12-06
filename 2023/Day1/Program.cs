// Task https://adventofcode.com/2023/day/1
Console.WriteLine("AoC 2023 - Day 1");

var inputValues = File.ReadAllLines("inputValues.txt");

var res1 = SolveFirstPart(inputValues);
var res2 = SolveSecondPart(inputValues);

Console.WriteLine($"Part 1: {res1}");
Console.WriteLine($"Part 2: {res2}");
Console.ReadKey();

static int SolveFirstPart(string[] input) =>
input
        .Select(line => ExtractCalibrationValuesFirstPart(line))
        .Sum();


static int SolveSecondPart(string[] input) =>
input
        .Select(line => ExtractCalibrationValuesSecondPart(line))
        .Sum();


static int ExtractCalibrationValuesFirstPart(string input)
{
    char firstDigitChar = input.First(char.IsDigit);
    char lastDigitChar = input.Last(char.IsDigit);

    int firstDigit = int.Parse(firstDigitChar.ToString());
    int lastDigit = int.Parse(lastDigitChar.ToString());

    return firstDigit * 10 + lastDigit;
}

static int ExtractCalibrationValuesSecondPart(string input)
{
    Dictionary<string, int> wordToDigitLookup = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 }
        };

    List<int> digitsInString = new List<int>();


    for (int i = 0; i < input.Length; i++)
    {
        char character = input[i];

        if (char.IsDigit(character))
        {
            int digit = int.Parse(character.ToString());
            digitsInString.Add(digit); 
        }
        else
        {
            foreach (var entry in wordToDigitLookup)
            {
                var substr = input.Substring(i).ToLower();
                if (substr.StartsWith(entry.Key))
                {
                    digitsInString.Add(entry.Value);
                    break;
                }
            }
        }
    }

    return digitsInString.First() * 10 + digitsInString.Last();
}