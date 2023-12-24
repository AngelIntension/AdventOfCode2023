using System.Collections.Immutable;
using static System.Math;

namespace GearRatio
{
    public static class GearRatio {
        public static ImmutableArray<Part> ToParts(this Schematic @this) {
            var grid = @this.Content
                            .Split('\n')
                            .Select(line => line.ToCharArray().ToImmutableArray())
                            .ToImmutableArray();

            var indices = Enumerable.Range(0, grid[0].Length - 1).ToImmutableArray();
            var symbolCoordinates = Enumerable.Empty<Coordinate>().ToImmutableArray();
            for (var row = 0; row < grid.Length; row++) {
                symbolCoordinates = symbolCoordinates
                         .Concat(indices
                                .Zip(grid[row])
                                .Where(tuple => tuple.Second.IsPartSymbol())
                                 // ReSharper disable once AccessToModifiedClosure
                                .Select(tuple => new Coordinate(row, tuple.First)))
                         .ToImmutableArray();
            }

            var partCoordinates = new HashSet<Coordinate>();
            partCoordinates =
                symbolCoordinates
                   .Aggregate(
                        partCoordinates,
                        (current, coordinate) => current.Concat(grid.GetAdjacentPartCoordinates(coordinate))
                                                        .ToHashSet());

            return partCoordinates
                  .Select(coordinate => new Part(grid.GetNumberAt(coordinate)))
                  .ToImmutableArray();
        }

        private static bool IsPartSymbol(this char @this) => !int.TryParse(@this.ToString(), out _) && @this != '.';

        private static ImmutableArray<Coordinate> GetAdjacentPartCoordinates(
            this ImmutableArray<ImmutableArray<char>> @this,
            Coordinate center) {
            var coordinates = new List<Coordinate>();
            for (var row = Max(0, center.Row - 1); row <= Min(@this.Length - 1, center.Row + 1); row++) {
                for (var column = Max(0, center.Column - 1); column <= Min(@this[row].Length -1, center.Column + 1); column++) {
                    if (@this[row][column].IsDigit()) {
                        var startOfPartNumber = column;
                        while (startOfPartNumber > 0) {
                            if (!@this[row][startOfPartNumber - 1].IsDigit()) {
                                break; }
                            startOfPartNumber--;
                        }

                        var coordinate = new Coordinate(row, startOfPartNumber);
                        coordinates.Add(coordinate);
                    }
                }
            }

            return coordinates.ToImmutableArray();
        }

        private static int GetNumberAt(this ImmutableArray<ImmutableArray<char>> @this, Coordinate coordinate) {
            var row = coordinate.Row;
            var column = coordinate.Column;
            var numberString = @this[row]
                              .Slice(column, @this[row].Length - column)
                              .TakeWhile(c => c.IsDigit())
                              .ToArray()
                              .AsSpan()
                              .ToString();
            return int.Parse(numberString);
        }

        private static bool IsDigit(this char @this) => int.TryParse(@this.ToString(), out _);

        private record struct Coordinate(int Row, int Column) {
            public int Row { get; init; } = Row;
            public int Column { get; init; } = Column; }
    }


    public record struct Part(int Number) {
        public int Number { get; init; } = Number;
    }

    public record struct Schematic(string Content) {
        public string Content { get; init; } = Content;
    }
}
