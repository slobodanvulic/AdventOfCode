// Task https://adventofcode.com/2023/day/12
Console.WriteLine("AoC 2023 - Day 12");

var inputData = File.ReadAllLines("inputValues1.txt");

// This cache is used to reduce number of repeated calculation (recursions)
// Dynamic programming technique
var cache = new Dictionary<(int, int, int), long>();

var firstPart = 0L;

foreach (var line in inputData)
{
    var parts = line.Split();
    var springs = parts[0];
    var nums = parts[1].Split(',').Select(int.Parse).ToList();

    firstPart += Calculate(springs, 0, nums, 0, 0, cache);
    cache.Clear();
}

Console.WriteLine($"First part {firstPart}");



var secondPart = 0L;

foreach (var line in inputData)
{
    var parts = line.Split();
    var springs = string.Join("?", parts[0], parts[0], parts[0], parts[0], parts[0]);
    var numsStr = string.Join(",", parts[1], parts[1], parts[1], parts[1], parts[1]);
    var nums = numsStr.Split(',').Select(int.Parse).ToList();

    secondPart += Calculate(springs, 0, nums,  0, 0, cache);
    cache.Clear();
}

Console.WriteLine($"Second part {secondPart}");
Console.ReadKey();
static long Calculate(string springs, int springIndex, List<int> nums, int numIndex, int blockLen, Dictionary<(int, int, int), long> cache)
{
    if (cache.TryGetValue((springIndex, numIndex, blockLen), out var value))
    {
        return value;
    }

    var ret = 0L;

    if (springIndex == springs.Length)
    {
        if (numIndex == nums.Count && blockLen == 0)
            return 1;
        if (numIndex == nums.Count - 1 && nums[numIndex] == blockLen)
            return 1;

        return 0;
    }

    foreach ( var spring in new[] { '.', '#' })
    {
        if (springs[springIndex] != spring && springs[springIndex] != '?') // block of #
            continue;

        if (spring == '.' && blockLen == 0)
            ret += Calculate(springs, springIndex + 1, nums, numIndex, 0, cache);
        else if (spring == '.' && blockLen > 0 && numIndex < nums.Count && nums[numIndex] == blockLen)
            ret += Calculate(springs, springIndex + 1, nums, numIndex + 1, 0, cache);
        else if (spring == '#')
            ret += Calculate(springs, springIndex + 1, nums, numIndex, blockLen + 1, cache);
    }

    cache[(springIndex, numIndex, blockLen)] = ret;

    return ret;
};



