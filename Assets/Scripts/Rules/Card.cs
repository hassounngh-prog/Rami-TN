using System;

namespace Rami.Rules
{
    public enum BackColour { Red, Blue }

    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    public enum Rank
    {
        Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
    }

    public sealed class Card : IEquatable<Card>
    {
        public Suit? Suit { get; }
        public Rank? Rank { get; }
        public BackColour Back { get; }
        public bool IsJoker => !Suit.HasValue;

        private Card(Suit? suit, Rank? rank, BackColour back)
        {
            Suit = suit;
            Rank = rank;
            Back = back;
        }

        public static Card Real(Suit suit, Rank rank, BackColour back) => new Card(suit, rank, back);

        public static Card Joker(BackColour back) => new Card(null, null, back);

        public bool Equals(Card other) =>
            !ReferenceEquals(other, null) && Suit == other.Suit && Rank == other.Rank && Back == other.Back;

        public override bool Equals(object obj) => Equals(obj as Card);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)Back;
                hash = hash * 31 + (Suit.HasValue ? (int)Suit.Value + 1 : 0);
                hash = hash * 31 + (Rank.HasValue ? (int)Rank.Value + 1 : 0);
                return hash;
            }
        }
    }
}