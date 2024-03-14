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
    }
}
