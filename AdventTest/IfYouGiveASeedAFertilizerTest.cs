using FluentAssertions;
using IfYouGiveASeedAFertilizer;

namespace AdventTest {
    public class IfYouGiveASeedAFertilizerTest {
        private static string TestAlmanacPath => @"D:\Projects\Code\AdventOfCode2023\AdventTest\TestAlmanac.txt";

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
        public static void CreateTransform_ShouldReturnClosure_WhichCorrectlyComputesTransforms(
            string mapName, int input, int expected) {
            var almanacText = File.ReadAllText(Path.GetFullPath(TestAlmanacPath));
            var almanac = new Almanac(almanacText);
            var map = almanac.CreateMap(mapName);
            map(input).Should().Be(expected);
        }
    }
}
