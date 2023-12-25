using System.Collections.Immutable;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace IfYouGiveASeedAFertilizer; 

public static class FarmersAlmanac {
    public static Func<int, int> CreateMap(this Almanac @this, string mapName) {
        var almanacMaps = @this.AlmanacMaps.Where(map => map.MapName == mapName).ToImmutableArray();
        return a => Maybe.From(almanacMaps)
                         .Bind(maps => {
                              bool IsApplicable(AlmanacMap map) => a >= map.SourceStart && a < map.SourceStart + map.Count;
                              return maps.Any(IsApplicable)
                                  ? maps.First(IsApplicable)
                                  : Maybe<AlmanacMap>.None; })
                         .Match(
                              None: () => a,
                              Some: map => map.DestinationStart + (a - map.SourceStart));
    }
}

public record struct Almanac {
    public ImmutableHashSet<AlmanacMap> AlmanacMaps { get; init; }

    public Almanac(string text) {
        var lines = text.Split('\n')
                        .Select(line => line.Trim())
                        .Where(line => line != string.Empty)
                        .ToImmutableArray();
        var mapNames = GetMapNames(lines);
        AlmanacMaps = mapNames.SelectMany(mapName => GetAlmanacMaps(lines, mapName))
                              .ToImmutableHashSet();
    }

    internal static ImmutableHashSet<string> GetMapNames(IEnumerable<string> mapDefinitions) {
        var mapNameRegex = new Regex(@"(?'mapName'\w+-to-\w+)\s+map:");
        return mapDefinitions.Select(line => mapNameRegex.Match(line).Groups["mapName"].Value)
                             .Where(mapName => mapName != string.Empty).ToImmutableHashSet();
    }

    internal static ImmutableHashSet<AlmanacMap> GetAlmanacMaps(IEnumerable<string> lines, string mapName) {
        var mapNameRegex = new Regex(@"(?'mapName'\w+-to-\w+)\s+map:");
        var mapRangeRegex = new Regex(@"(?'destinationStartIndex'\d+)\s+(?'sourceStartIndex'\d+)\s+(?'count'\d+)");
        return lines
              .SkipWhile(line => {
                   if (!mapNameRegex.IsMatch(line)) {
                       return true;
                   }
                   return mapNameRegex.Match(line).Groups["mapName"].Value != mapName; })
              .Skip(1)
              .TakeWhile(line => mapRangeRegex.IsMatch(line)).Select(mapRange => {
                   var match = mapRangeRegex.Match(mapRange);
                   var sourceStartIndex = int.Parse(match.Groups["sourceStartIndex"].Value);
                   var destinationStartIndex = int.Parse(match.Groups["destinationStartIndex"].Value);
                   var count = int.Parse(match.Groups["count"].Value);
                   return new AlmanacMap(mapName, sourceStartIndex, destinationStartIndex, count); })
              .ToImmutableHashSet();
    }
}

public record struct AlmanacMap(string MapName, int SourceStart, int DestinationStart, int Count);