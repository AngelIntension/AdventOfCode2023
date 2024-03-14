using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

[assembly: InternalsVisibleTo("AdventTest")]

namespace IfYouGiveASeedAFertilizer; 

public static class FarmersAlmanac {
    public static BigInteger GetLocationFor(this Almanac @this, BigInteger seed)
        => Maybe.From(seed)
                .Map(@this.SeedToSoil)
                .Map(@this.SoilToFertilizer)
                .Map(@this.FertilizerToWater)
                .Map(@this.WaterToLight)
                .Map(@this.LightToTemperature)
                .Map(@this.TemperatureToHumidity)
                .Map(@this.HumidityToLocation)
                .Value;

    internal static Regex MapNameRegex => new(@"(?'mapName'\w+-to-\w+)\s+map:");

    internal static Func<BigInteger, BigInteger> CreateMap(this Almanac @this, string mapName) {
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

    internal static ImmutableHashSet<string> GetMapNames(this IEnumerable<string> @this)
        => @this.Select(line => MapNameRegex.Match(line).Groups["mapName"].Value)
                .Where(mapName => mapName != string.Empty)
                .ToImmutableHashSet();

    internal static ImmutableHashSet<SeedRange> ParseSeedRanges(this string @this) {
        var seedRangeRegex = new Regex(@"(?'start'\d+)\s+(?'count'\d+)(?'rest'.*)?");
        var ranges = Enumerable.Empty<SeedRange>().ToImmutableHashSet();
        var str = @this.TrimStart("seeds: ".ToCharArray());
        while (seedRangeRegex.IsMatch(str)) {
            var match = seedRangeRegex.Match(str);
            var start = BigInteger.Parse(match.Groups["start"].Value);
            var end = start + BigInteger.Parse(match.Groups["count"].Value) - 1;
            ranges = ranges.Add(new SeedRange(start, end));
            str = match.Groups["rest"].Value;
        }
        return ranges;
    }

    internal static ImmutableHashSet<AlmanacMap> GetAlmanacMaps(this IEnumerable<string> @this, string mapName) {
        var mapRangeRegex = new Regex(@"(?'destinationStartIndex'\d+)\s+(?'sourceStartIndex'\d+)\s+(?'count'\d+)");
        return @this
              .SkipWhile(line => {
                   if (!MapNameRegex.IsMatch(line)) {
                       return true;
                   }
                   return MapNameRegex.Match(line).Groups["mapName"].Value != mapName;
               })
              .Skip(1)
              .TakeWhile(line => mapRangeRegex.IsMatch(line))
              .Select(mapRange => {
                   var match = mapRangeRegex.Match(mapRange);
                   var sourceStartIndex = BigInteger.Parse(match.Groups["sourceStartIndex"].Value);
                   var destinationStartIndex = BigInteger.Parse(match.Groups["destinationStartIndex"].Value);
                   var count = BigInteger.Parse(match.Groups["count"].Value);
                   return new AlmanacMap(mapName, sourceStartIndex, destinationStartIndex, count);
               })
              .ToImmutableHashSet();
    }
}

public record struct Almanac {
    public ImmutableHashSet<SeedRange> SeedRanges { get; init; }

    public Almanac(string text) {
        var lines = text.Split('\n')
                        .Select(line => line.Trim())
                        .Where(line => line != string.Empty)
                        .ToImmutableArray();
        AlmanacMaps = lines.GetMapNames()
                           .SelectMany(lines.GetAlmanacMaps)
                           .ToImmutableHashSet();
        SeedToSoil = this.CreateMap("seed-to-soil");
        SoilToFertilizer = this.CreateMap("soil-to-fertilizer");
        FertilizerToWater = this.CreateMap("fertilizer-to-water");
        WaterToLight = this.CreateMap("water-to-light");
        LightToTemperature = this.CreateMap("light-to-temperature");
        TemperatureToHumidity = this.CreateMap("temperature-to-humidity");
        HumidityToLocation = this.CreateMap("humidity-to-location");
        SeedRanges = lines.First().ParseSeedRanges();
    }

    internal ImmutableHashSet<AlmanacMap> AlmanacMaps { get; init; }
    internal Func<BigInteger, BigInteger> SeedToSoil { get; init; }
    internal Func<BigInteger, BigInteger> SoilToFertilizer { get; init; }
    internal Func<BigInteger, BigInteger> FertilizerToWater { get; init; }
    internal Func<BigInteger, BigInteger> WaterToLight { get; init; }
    internal Func<BigInteger, BigInteger> LightToTemperature { get; init; }
    internal Func<BigInteger, BigInteger> TemperatureToHumidity { get; init; }
    internal Func<BigInteger, BigInteger> HumidityToLocation { get; init; }
}

internal record struct AlmanacMap(string MapName, BigInteger SourceStart, BigInteger DestinationStart, BigInteger Count);
public record struct SeedRange(BigInteger Start, BigInteger End);