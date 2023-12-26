
using System.Collections.Concurrent;
using System.Collections.Immutable;
using IfYouGiveASeedAFertilizer;
using System.Numerics;

const string testAlmanacTxt = @"D:\Projects\Code\AdventOfCode2023\AdventTest\TestAlmanac.txt";
const string inputAlmanacTxt = @"D:\Projects\Code\AdventOfCode2023\IfYouGiveASeedAFertilizer\InputAlmanac.txt";
var almanacText = File.ReadAllText(Path.GetFullPath(inputAlmanacTxt));
var almanac = new Almanac(almanacText);
var nearestLocations = new ConcurrentDictionary<Guid, BigInteger>();
var tasks = Enumerable.Empty<Task>().ToImmutableArray();
var seedRanges = almanac.SeedRanges
                        .OrderByDescending(range => range.End - range.Start)
                        .ToImmutableArray();
while (seedRanges.Length < Environment.ProcessorCount) {
    var range = seedRanges.First();
    var split = range.Start + (range.End - range.Start) / 2;
    var left = range with { End = split };
    var right = range with { Start = split + 1 };
    seedRanges = seedRanges
                .Append(left)
                .Append(right)
                .OrderByDescending(r => r.End - r.Start)
                .ToImmutableArray();
}

foreach (var seedRange in seedRanges) {
    var task = new Task(() => {
        var taskId = Guid.NewGuid();
        nearestLocations[taskId] = almanac.GetLocationFor(seedRange.Start);
        Console.WriteLine($"started processing range ({seedRange.Start}, {seedRange.End})...");
        for (var seed = seedRange.Start + 1; seed <= seedRange.End; seed++) {
            nearestLocations[taskId] = BigInteger.Min(nearestLocations[taskId], almanac.GetLocationFor(seed));
        }
        Console.WriteLine($"nearest location for thread {taskId}: {nearestLocations[taskId]}");
    });
    tasks = tasks.Add(task);
    task.Start();
}

Task.WaitAll(tasks.ToArray());
Console.WriteLine($"nearest location from all seeds: {nearestLocations.Min(kvp => kvp.Value)}");

Console.WriteLine();
Console.ReadLine();
