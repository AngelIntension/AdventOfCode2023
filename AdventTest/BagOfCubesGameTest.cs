using System.Text.RegularExpressions;
using AutoFixture;
using BagOfCubesGame;
using FluentAssertions;

namespace AdventTest; 

public class BagOfCubesGameTest {
    public BagState TestBag { get; } = new(12, 13, 14);

    [Theory]
    [InlineData("3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)]
    [InlineData("1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", true)]
    [InlineData("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", false)]
    [InlineData("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", false)]
    [InlineData("6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", true)]
    public void IsValid_FunctionTest(string turnsList, bool expected)
        => new BagOfCubesGameState(turnsList, TestBag)
          .IsValid()
          .Should().Be(expected);

    [Fact]
    public void SumValidGames_GivenGamesToTestFile() {
        // ReSharper disable once UnusedVariable
        var sumOfValidGames = 
            GetGameTuplesFromFile()
               .Sum(tuple => {
                    var game = tuple.Item2;
                    var gameNumber = tuple.Item1;
                    return game.IsValid() ? gameNumber : 0; });
        Assert.True(true); }

    [Theory]
    [InlineData("3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 48)]
    [InlineData("1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12)]
    [InlineData("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 1560)]
    [InlineData("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 630)]
    [InlineData("6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 36)]
    public void GetMinimumBag_ShouldReturnBagWithExpectedPower(string turnsList, int expectedPowerValue)
        => new BagOfCubesGameState(turnsList, new Fixture().Create<BagState>())
          .GetMinimumBag()
          .GetPower()
          .Should().Be(new BagPower(expectedPowerValue));

    [Fact]
    public void SumPowerOfMinimumBags_GivenGamesToTestFile() {
        // ReSharper disable once UnusedVariable
        var sumMinimumBagPower = GetGameTuplesFromFile().Sum(tuple => tuple.Item2.GetMinimumBag().GetPower());
        Assert.True(true); }

    private IEnumerable<Tuple<int, BagOfCubesGameState>> GetGameTuplesFromFile() {
        var gamesToTest = Path.GetFullPath(@"D:\Projects\Code\AdventOfCode2023\BagOfCubesGame\GamesToTest.txt");
        var regex = new Regex(@"Game\s+(?'gameNumber'[0123456789]+):(?'turnsList'.*)");
        return File.ReadLines(gamesToTest)
                   .Select(line => {
                        var match = regex.Match(line);
                        var gameNumber = int.Parse(match.Groups["gameNumber"].Value);
                        var game = new BagOfCubesGameState(match.Groups["turnsList"].Value, TestBag);
                        return new Tuple<int, BagOfCubesGameState>(gameNumber, game); }); }
}