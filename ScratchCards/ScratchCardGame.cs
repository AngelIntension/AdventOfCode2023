using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ScratchCards; 

public static class ScratchCardGame {
    public static int Score(this ScratchCardGameState @this)
        => @this.WinningNumbers.Intersect(@this.PlayerNumbers).Count;

    public static int Score(this ScratchGameSet @this) {
        var buckets = Enumerable.Repeat(1, @this.Games.Count).ToArray();
        for (var number = 1; number < @this.Games.Count; number++) {
            var score = @this.Games[number].Score();
            for (var i = number; i < number + score; i++) {
                buckets[i] += buckets[number - 1];
            }
        }

        var finalCardCount = buckets.Sum();
        return finalCardCount;
    }

    public static ScratchGameSet GetGameSetFromFile(string path) {
        var enumeratedGames = File.ReadLines(Path.GetFullPath(path)).Select(ParseGame).ToImmutableArray();
        return new ScratchGameSet(enumeratedGames);
    }

    public static (int GameNumber, ScratchCardGameState Game) ParseGame(string line) {
        const string pattern
            = @"Card\s+(?'gameNumber'[0123456789]+):(?'winningNumbers'[\s0123456789]+)[|](?'playerNumbers'[\s0123456789]+)";
        var regex = new Regex(pattern);
        var match = regex.Match(line);
        var gameNumber = int.Parse(match.Groups["gameNumber"].Value);
        var winningNumbers = match.Groups["winningNumbers"].Value
                                  .Trim().Split(' ').Where(str => str != string.Empty)
                                  .Select(int.Parse)
                                  .ToImmutableHashSet();
        var playerNumbers = match.Groups["playerNumbers"].Value
                                 .Trim().Split(' ').Where(str => str != string.Empty)
                                 .Select(int.Parse)
                                 .ToImmutableHashSet();
        var game = new ScratchCardGameState(winningNumbers, playerNumbers);
        return (gameNumber, game);
    }
}

public record struct ScratchGameSet {
    public ScratchGameSet(IEnumerable<(int GameNumber, ScratchCardGameState Game)> enumeratedGames)
        => Games = enumeratedGames
           .ToImmutableSortedDictionary(
                tuple => tuple.GameNumber, 
                tuple => tuple.Game);

    public ImmutableSortedDictionary<int, ScratchCardGameState> Games { get; init; }
}

public record struct ScratchCardGameState(ImmutableHashSet<int> WinningNumbers, ImmutableHashSet<int> PlayerNumbers) {
    public ImmutableHashSet<int> WinningNumbers { get; init; } = WinningNumbers;
    public ImmutableHashSet<int> PlayerNumbers { get; init; } = PlayerNumbers;
}