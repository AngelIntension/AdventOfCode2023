using FluentAssertions;
using GearRatio;

namespace AdventTest {
    public class GearRatiosTest {
        private static string TestSchematicContent => "467..114..\n" +
                                                      "...*......\n" +
                                                      "..35..633.\n" +
                                                      "......#...\n" +
                                                      "617*......\n" +
                                                      ".....+.58.\n" +
                                                      "..592.....\n" +
                                                      "......755.\n" +
                                                      "...$.*....\n" +
                                                      ".664.598..";

        [Fact]
        public void ToParts_ShouldReturnCorrectPartsFromSchematic()
            => new Schematic(TestSchematicContent)
              .ToParts()
              .Should().BeEquivalentTo(
                   new List<Part> {
                       new(467),
                       new(35),
                       new(633),
                       new(617),
                       new(592),
                       new(755),
                       new(664),
                       new(598) });
    }
}
