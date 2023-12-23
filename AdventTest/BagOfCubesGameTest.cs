using BagOfCubesGame;
using FluentAssertions;

namespace AdventTest {
    public class BagOfCubesGameTest {
        public BagState TestBag { get; } = new(12, 13, 14);

        [Theory]
        [InlineData("3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)]
        [InlineData("1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", true)]
        [InlineData("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", false)]
        [InlineData("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", false)]
        [InlineData("6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", true)]
        public void IsValid_GivenTestBag_ShouldCorrectlyComputeGameValidity(string input, bool expected) {
            var game = new BagOfCubesGameState(input, TestBag);
            game.IsValid().Should().Be(expected);
        }
    }
}
