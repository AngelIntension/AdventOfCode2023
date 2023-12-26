using System.Numerics;
using FluentAssertions;
using IfYouGiveASeedAFertilizer;

namespace AdventTest {
    public class IfYouGiveASeedAFertilizerTest {
        private static string TestAlmanacPath => @"D:\Projects\Code\AdventOfCode2023\AdventTest\TestAlmanac.txt";
        private static string InputAlmanacPath => @"D:\Projects\Code\AdventOfCode2023\IfYouGiveASeedAFertilizer\InputAlmanac.txt";

        [Theory]
        [InlineData("seed-to-soil", 98, 50)]
        [InlineData("seed-to-soil", 99, 51)]
        [InlineData("seed-to-soil", 50, 52)]
        [InlineData("seed-to-soil", 51, 53)]
        [InlineData("seed-to-soil", 1, 1)]
        [InlineData("seed-to-soil", 10, 10)]
        [InlineData("soil-to-fertilizer", 15, 0)]
        [InlineData("soil-to-fertilizer", 52, 37)]
        [InlineData("soil-to-fertilizer", 0, 39)]
        public static void CreateMap_ShouldReturnClosure_WhichCorrectlyComputesMappings(
            string mapName, int input, int expected) {
            var almanacText = File.ReadAllText(Path.GetFullPath(TestAlmanacPath));
            var almanac = new Almanac(almanacText);
            var map = almanac.CreateMap(mapName);
            map(input).Should().Be(expected);
        }

        [Theory]
        [InlineData(79, 82)]
        [InlineData(14, 43)]
        [InlineData(55, 86)]
        [InlineData(13, 35)]
        public static void GetLocationFor_ShouldCorrectlyComputeMappedLocation(int seed, int expectedLocation) {
            var almanacText = File.ReadAllText(Path.GetFullPath(TestAlmanacPath));
            var almanac = new Almanac(almanacText);
            almanac.GetLocationFor(seed).Should().Be(expectedLocation);
        }

        [Fact]
        public static void ComputeNearestSeedLocationFromInputAlmanac() {
            var almanacText = File.ReadAllText(Path.GetFullPath(TestAlmanacPath));
            var almanac = new Almanac(almanacText);
            var nearestLocation = almanac.GetLocationFor(almanac.SeedRanges.First().Start);
            foreach (var seedRange in almanac.SeedRanges) {
                for (var seed = seedRange.Start; seed <= seedRange.End; seed++) {
                    nearestLocation = BigInteger.Min(nearestLocation, almanac.GetLocationFor(seed));
                }
            }
            nearestLocation.Should().Be(46);
        }
    }
}
