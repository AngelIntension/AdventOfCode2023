using System.Collections.Immutable;
using FluentAssertions;
using GearRatio;

namespace AdventTest {
    public static class GearRatioTest {
        private static string TestSchematicContent => "467..114..\n" +
                                                      "...*......\n" +
                                                      "..35..633.\n" +
                                                      "......#...\n" +
                                                      "617*......\n" +
                                                      ".....+.58.\n" +
                                                      "..592.....\n" +
                                                      "......755.\n" +
                                                      "...$.*....\n" +
                                                      ".664.598..\n" +
                                                      ".......123";

        private static string SchematicPath => @"D:\Projects\Code\AdventOfCode2023\GearRatio\Schematic.txt";

        [Fact]
        public static void ToParts_ShouldReturnCorrectPartsFromSchematic()
            => new Schematic(TestSchematicContent)
              .ToParts()
              .Should().BeEquivalentTo(
                   new Part[] {
                           new(467),
                           new(35),
                           new(633),
                           new(617),
                           new(592),
                           new(755),
                           new(664),
                           new(598) }
                      .ToImmutableArray());

        [Fact]
        public static void ComputeSumOfReferencedPartNumbers() {
            var parts = new Schematic(File.ReadAllText(Path.GetFullPath(SchematicPath))).ToParts();
            var sumOfPartNumbers = parts.Sum(part => part.Number);
            Assert.True(true);
        }
    }
}
