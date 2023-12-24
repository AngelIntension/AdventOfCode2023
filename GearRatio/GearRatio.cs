using System.Collections.Immutable;
using static System.Math;

namespace GearRatio
{
    public static class GearRatio {
        public static ImmutableArray<Part> ToParts(this Schematic @this) {
            var parts = Enumerable.Empty<Part>().ToImmutableArray();
            var grid = @this.Content
                            .Split('\n')
                            .Select(line => line.ToCharArray().ToImmutableArray())
                            .ToImmutableArray();
            for (var row = 0; row < grid.Length; row++) {
                var start = 0;
                while (start < grid[row].Length) {
                    (start, var end) = grid[row].GetEndpointsOfNextNumber(start);
                    if (start == -1 || !grid.ContainsAdjacentPartSymbol(row, start, end)) {
                        break; }

                    var partNumber = int.Parse(grid[row].AsSpan(start, end - start + 1).ToString());
                    parts = parts.Append(new Part(partNumber)).ToImmutableArray();
                    start = end + 1; } }
            return parts; }

        private static (int, int) GetEndpointsOfNextNumber(this ImmutableArray<char> @this, int startIndex) {
            var start = startIndex;
            while (start < @this.Length) {
                if (int.TryParse(@this[start].ToString(), out _)) {
                    break; }
                start++; }
            if (start >= @this.Length) {
                return (-1, -1); }
            var end = start + 1;
            while (end < @this.Length) {
                if (!int.TryParse(@this[end].ToString(), out _)) {
                    end--;
                    break; }
                end++; }
            return end >= @this.Length ? (start, end - 1) : (start, end); }

        private static bool ContainsAdjacentPartSymbol(
            this ImmutableArray<ImmutableArray<char>> @this,
            int row,
            int start,
            int end) {
            var surroundingCharacters = Enumerable.Empty<char>().ToImmutableArray();
            var clampedStart = Max(0, start - 1);
            var clampedEnd = Min(@this[row].Length - 1, end + 1);

            if (row > 0) {
                surroundingCharacters = @this[row - 1]
                     .AsSpan(clampedStart, clampedEnd - clampedStart + 1)
                     .ToImmutableArray(); }

            surroundingCharacters = surroundingCharacters
                                   .Append(@this[row][clampedStart])
                                   .Append(@this[row][clampedEnd])
                                   .ToImmutableArray();

            if (row < @this.Length - 1) {
                surroundingCharacters = surroundingCharacters
                                       .Concat(@this[row + 1]
                                              .AsSpan(clampedStart, clampedEnd - clampedStart + 1)
                                              .ToArray())
                                       .ToImmutableArray(); }
            return surroundingCharacters.Any(c => c.IsPartSymbol()); }

        private static bool IsPartSymbol(this char @this) => !int.TryParse(@this.ToString(), out _) && @this != '.';
    }

    public record struct Part(int Number) {
        public int Number { get; init; } = Number;
    }

    public record struct Schematic(string Content) {
        public string Content { get; init; } = Content;
    }
}
