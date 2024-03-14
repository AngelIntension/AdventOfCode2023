namespace CamelCards
{
    public record Hand(string Cards);

    public static class CamelCards
    {
        public static HandType Type(this Hand @this) =>
            @this.Split()
                 .OrderByDescending(group => group.Count)
                 .ToArray()
                switch {
                    [{ Count: 5 }] => HandType.FiveOfAKind,
                    [{ Count: 4 }, _] => HandType.FourOfAKind,
                    [{ Count: 3 }, { Count: 2 }] => HandType.FullHouse,
                    [{ Count: 3 }, { Count: 1 }, { Count: 1 }] => HandType.ThreeOfAKind,
                    [{ Count: 2 }, { Count: 2 }, _] => HandType.TwoPair,
                    [{ Count: 2 }, { Count: 1 }, { Count: 1 }, _] => HandType.OnePair,
                    _ => HandType.HighCard };

        internal static Group[] Split(this Hand @this) => (
                from rank in @this.Cards.ToCharArray().Distinct()
                let count = @this.Cards.ToCharArray().Count(card => card == rank)
                select new Group(rank, count))
           .ToArray();
    }

    public enum HandType {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }

    internal record Group(char Rank, int Count);
}
