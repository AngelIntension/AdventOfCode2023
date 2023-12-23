using System.Text.RegularExpressions;

namespace BagOfCubesGame; 

public static class BagOfCubesGame {
    public static bool IsValid(this BagOfCubesGameState @this)
        => @this.TurnsList
                .Split(';')
                .Select(turn => new BagOfCubesGameState(turn, @this.Bag))
                .Select(game => game.TurnsList.IsValid(game.Bag))
                .Aggregate((a, b) => a && b);

    private static bool IsValid(this string @this, BagState bag) {
        if(@this.Contains(';')) {
            throw new ArgumentException("string must contain a single turn"); }

        var redCount = GetMatchingCount("red");
        var greenCount = GetMatchingCount("green");
        var blueCount = GetMatchingCount("blue");

        return redCount <= bag.RedCount
            && greenCount <= bag.GreenCount
            && blueCount <= bag.BlueCount;

        int GetMatchingCount(string color) {
            var regex = new Regex($"(?'number'[0123456789]+)\\s+(?:{color})");
            int.TryParse(regex.Match(@this).Groups["number"].Value, out var count);
            return count;
        }
    }
}

public record struct BagOfCubesGameState(string TurnsList, BagState Bag) {
    public string TurnsList { get; init; } = TurnsList.Trim().ToLower();
    public BagState Bag { get; init; } = Bag;
}

public record struct BagState(int RedCount, int GreenCount, int BlueCount) {
    public int RedCount { get; init; } = RedCount;
    public int GreenCount { get; init; } = GreenCount;
    public int BlueCount { get; init; } = BlueCount;
}