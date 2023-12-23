using System.Text.RegularExpressions;
using static System.Math;

namespace BagOfCubesGame; 

public static class BagOfCubesGame {
    public static bool IsValid(this BagOfCubesGameState @this)
        => @this.TurnsList
                .Split(';')
                .Select(turn => new BagOfCubesGameState(turn, @this.Bag))
                .Select(game => game.TurnsList.IsValid(game.Bag))
                .Aggregate((a, b) => a && b);

    public static BagState GetMinimumBag(this BagOfCubesGameState @this)
        => @this.TurnsList
                .Split(';')
                .Aggregate(
                     new BagState(0, 0, 0),
                     (accumulator, turn) => new BagState(
                         Max(turn.GetColorCount("red"), accumulator.RedCount),
                         Max(turn.GetColorCount("green"), accumulator.GreenCount),
                         Max(turn.GetColorCount("blue"), accumulator.BlueCount)));

    public static BagPower GetPower(this BagState @this) => new(@this.RedCount * @this.GreenCount * @this.BlueCount);

    private static bool IsValid(this string @this, BagState bag) {
        if(@this.Contains(';')) {
            throw new ArgumentException("string must contain a single turn"); }
        return @this.GetColorCount("red") <= bag.RedCount
            && @this.GetColorCount("green") <= bag.GreenCount
            && @this.GetColorCount("blue") <= bag.BlueCount; }

    private static int GetColorCount(this string @this, string color) {
        var regex = new Regex($"(?'number'[0123456789]+)\\s+{color}");
        int.TryParse(regex.Match(@this).Groups["number"].Value, out var count);
        return count; }
}

public record struct BagOfCubesGameState(string TurnsList, BagState Bag) {
    public string TurnsList { get; init; } = TurnsList.Trim().ToLower();
    public BagState Bag { get; init; } = Bag; }

public record struct BagState(int RedCount, int GreenCount, int BlueCount) {
    public int RedCount { get; init; } = RedCount;
    public int GreenCount { get; init; } = GreenCount;
    public int BlueCount { get; init; } = BlueCount; }

public record struct BagPower(int Value) {
    public int Value { get; init; } = Value;
    public static implicit operator int(BagPower power) => power.Value;
    public static implicit operator BagPower(int value) => new(value); }