namespace CamelCards
{
    public record Hand {
        internal string Cards { get; }

        public Hand(string cards) {
            Cards = cards.ToUpper();
        }
    }

    public static class CamelCards
    {
        public static HandType Type(this Hand @this) =>
            @this.GetGroupsByRank()
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

        public static bool Beats(this Hand left, Hand right) {
            if (left.Type() != right.Type()) {
                return left.Type() > right.Type();
            }

            var mismatchedPair = left.Cards
                                     .ToCharArray()
                                     .Zip(right.Cards.ToCharArray())
                                     .SkipWhile(tuple => tuple.First.Rank() == tuple.Second.Rank())
                                     .First();
            return mismatchedPair.First.Rank() > mismatchedPair.Second.Rank();
        }

        internal static Group[] GetGroupsByRank(this Hand @this) => (
                from rank in @this.Cards.ToCharArray().Distinct()
                let count = @this.Cards.ToCharArray().Count(card => card == rank)
                select new Group(rank, count))
           .ToArray();

        internal static int Rank(this char @this) {
            return @this switch {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => int.Parse(@this.ToString())
            };
        }
    }

    public enum HandType {
        FiveOfAKind = 6,
        FourOfAKind = 5,
        FullHouse = 4,
        ThreeOfAKind = 3,
        TwoPair = 2,
        OnePair = 1,
        HighCard = 0
    }

    internal record Group(char Rank, int Count);
}
