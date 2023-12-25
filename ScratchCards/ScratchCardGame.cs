using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ScratchCards
{
    public static class ScratchCardGame {

        public static ImmutableArray<(int GameNumber, ScratchCardGameState Game)> GetGamesFromFile(string path)
            => File.ReadLines(Path.GetFullPath(path))
                   .Select(ParseGame)
                   .ToImmutableArray();

        public static (int GameNumber, ScratchCardGameState Game) ParseGame(string line) {
            const string pattern = @"Card\s+(?'gameNumber'[0123456789]+):(?'winningNumbers'[\s0123456789]+)[|](?'playerNumbers'[\s0123456789]+)";
            var regex = new Regex(pattern);
            var match = regex.Match(line);
            var gameNumber = int.Parse(match.Groups["gameNumber"].Value);
            var winningNumbers = match.Groups["winningNumbers"].Value.Trim().Split(' ').Where(str => str != string.Empty).Select(int.Parse).ToImmutableHashSet();
            var playerNumbers = match.Groups["playerNumbers"].Value.Trim().Split(' ').Where(str => str != string.Empty)
                                     .Select(int.Parse).ToImmutableHashSet();
            var game = new ScratchCardGameState(winningNumbers, playerNumbers);
            return (gameNumber, game);
        }

        public static int Score(this ScratchCardGameState @this)
            => (int)Math.Pow(2, @this.WinningNumbers.Intersect(@this.PlayerNumbers).Count - 1);
    }

    public record struct ScratchCardGameState(ImmutableHashSet<int> WinningNumbers, ImmutableHashSet<int> PlayerNumbers) {
        public ImmutableHashSet<int> WinningNumbers { get; init; } = WinningNumbers;
        public ImmutableHashSet<int> PlayerNumbers { get; init; } = PlayerNumbers;
    }
}
