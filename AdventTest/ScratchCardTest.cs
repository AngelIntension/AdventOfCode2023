
using System.Collections.Immutable;
using FluentAssertions;
using ScratchCards;

namespace AdventTest {
    public class ScratchCardTest {
        private static string TestCardsFilepath => @"D:\Projects\Code\AdventOfCode2023\AdventTest\TestScratchCards.txt";
        private static string InputFilepath => @"D:\Projects\Code\AdventOfCode2023\ScratchCards\Input.txt";

        [Theory]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", 4)]
        [InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", 2)]
        [InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", 2)]
        [InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", 1)]
        [InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", 0)]
        [InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 0)]
        public static void Score_ShouldCorrectlyScoreGivenGame(string input, int expectedScore) {
            ScratchCardGame.ParseGame(input)
                           .Game
                           .Score()
                           .Should().Be(expectedScore);
        }

        [Fact]
        public static void ComputeSumOfGameScoresFromInput() {
            // ReSharper disable once UnusedVariable
            var sumOfGameScores = ScratchCardGame
                                 .GetGamesFromFile(InputFilepath)
                                 .Select(tuple => tuple.Game.Score())
                                 .Sum();
            Assert.True(true);
        }

        [Fact]
        public static void ComputeSumUsingCopyRule() {
            var tuples = ScratchCardGame.GetGamesFromFile(InputFilepath)
                                        .OrderBy(tuple => tuple.GameNumber)
                                        .ToImmutableArray();
            var buckets = Enumerable.Repeat(1, tuples.Length).ToArray();
            for (var number = 1; number < tuples.Max(t => t.GameNumber); number++) {
                var countOfCardsWithCurrentNumber = buckets[number - 1];
                var score = tuples[number - 1].Game.Score();
                for (var i = number; i < number + score; i++) {
                    buckets[i] += countOfCardsWithCurrentNumber;
                }
            }
            // ReSharper disable once UnusedVariable
            var finalCardCount = buckets.Sum();
            Assert.True(false);
        }
    }
}
