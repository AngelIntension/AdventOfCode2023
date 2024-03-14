using System.Reflection.Metadata;
using CamelCards;
using FluentAssertions;

namespace AdventTest {
    public class CamelCardsTest {
        [Theory]
        [InlineData("AAAAA", HandType.FiveOfAKind)]
        [InlineData("AA8AA", HandType.FourOfAKind)]
        [InlineData("23332", HandType.FullHouse)]
        [InlineData("TTT98", HandType.ThreeOfAKind)]
        [InlineData("23432", HandType.TwoPair)]
        [InlineData("A23A4", HandType.OnePair)]
        [InlineData("23456", HandType.HighCard)]
        public void Type_Should_ReturnCorrectHandType(string cards, HandType expected) =>
            new Hand(cards).Type().Should().Be(expected);

        [Theory]
        [InlineData("AAAAA", "AA8AA", true)]
        [InlineData("AA8AA", "AAAAA", false)]
        [InlineData("AA8AA", "23332", true)]
        [InlineData("23332", "TTT98", true)]
        [InlineData("TTT98", "23432", true)]
        [InlineData("23432", "A23A4", true)]
        [InlineData("A23A4", "23456", true)]
        [InlineData("23456", "34567", false)]
        [InlineData("33332", "2AAAA", true)]
        [InlineData("77888", "77788", true)]
        public void Beats_Should_CorrectlyCompareHands(string cards1, string cards2, bool expected) =>
            new Hand(cards1).Beats(new Hand(cards2)).Should().Be(expected);
    }
}
