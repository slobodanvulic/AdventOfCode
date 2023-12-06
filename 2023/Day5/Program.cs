// Task https://adventofcode.com/2023/day/5
Console.WriteLine("AoC 2023 - Day 5");

var inputData = File.ReadAllText("inputValues.txt");
var inputs = inputData.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

List<long> seeds = inputs[0][7..].Split(" ").Select(x => long.Parse(x)).ToList();
List<Record> seedToSoil = GetData(inputs[1]);
List<Record> soilToFertilizer = GetData(inputs[2]);
List<Record> fertilizerToWater = GetData(inputs[3]);
List<Record> waterToLight = GetData(inputs[4]);
List<Record> lightToTemperature = GetData(inputs[5]);
List<Record> temperatureToHumidity = GetData(inputs[6]);
List<Record> humidityToLocation = GetData(inputs[7]);

//First part
long minLocation = long.MaxValue;

foreach (long seed in seeds)
{
    List<long> map = new(){ seed };

    MapNext(map, seedToSoil);
    MapNext(map, soilToFertilizer);
    MapNext(map, fertilizerToWater);
    MapNext(map, waterToLight);
    MapNext(map, lightToTemperature);
    MapNext(map, temperatureToHumidity);
    MapNext(map, humidityToLocation);

    minLocation = Math.Min(map.Last(), minLocation);
}

Console.WriteLine($"Part one: {minLocation}");
minLocation = long.MaxValue;

// Second Part

var seedsRanges = seeds;
var tasks = new List<Task<long>>();

for (int j = 0; j < seedsRanges.Count; j += 2)
{
    var startSeed = seedsRanges[j];
    var endSeed = seedsRanges[j] + seedsRanges[j + 1];

    tasks.Add(Task.Run(() => FindMinLocation(seedToSoil,
        soilToFertilizer,
        fertilizerToWater,
        waterToLight,
        lightToTemperature,
        temperatureToHumidity,
        humidityToLocation,
        startSeed,
        endSeed)));
}

Task.WaitAll(tasks.ToArray());
Console.WriteLine($"Part two: {tasks.Select(t => t.Result).Min()}");
Console.ReadKey();

static long FindMinLocation(List<Record> seedToSoilMap,
    List<Record> soilToFertilizerMap,
    List<Record> fertilizerToWaterMap,
    List<Record> waterToLightMap,
    List<Record> lightToTemperatureMap,
    List<Record> temperatureToHumidityMap,
    List<Record> humidityToLocationMap,
    long seedRangeStart,
    long seedRangeEnd)
{
    long localMin = long.MaxValue;

    for (long k = seedRangeStart; k < seedRangeEnd; k++)
    {
        List<long> map = new() { k };

        MapNext(map, seedToSoilMap);
        MapNext(map, soilToFertilizerMap);
        MapNext(map, fertilizerToWaterMap);
        MapNext(map, waterToLightMap);
        MapNext(map, lightToTemperatureMap);
        MapNext(map, temperatureToHumidityMap);
        MapNext(map, humidityToLocationMap);

        localMin = Math.Min(map.Last(), localMin);
    }

    return localMin;
}

static List<Record> GetData(string line) =>
    line
        .Split(Environment.NewLine)
        .Skip(1)
        .Select(x =>
        {
            var nums = x.Split(" ");
            return new Record()
            {
                Destination = long.Parse(nums[0]),
                Source = long.Parse(nums[1]),
                Range = long.Parse(nums[2]),
            };
        }).ToList(); ;


static void MapNext(List<long> map, List<Record> seedToSoilMap)
{
    var matchingSoilData = seedToSoilMap.Where(x => x.Source <= map.Last() && x.Source + x.Range >= map.Last()).FirstOrDefault();
    if (matchingSoilData is null)
    {
        map.Add(map.Last());
    }
    else
    {
        map.Add(matchingSoilData.Destination + map.Last() - matchingSoilData.Source);
    }
}

internal class Record
{
    public long Destination { get; set; }
    public long Source { get; set; }
    public long Range { get; set; }
}

