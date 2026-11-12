using System;
using System.Collections.Generic;

namespace Rami.Rules
{
    public enum RunOrientation { Low, High }

    public enum CombinationKind { Msaker, Ma7loul }

    public sealed class RepresentedCard
    {
        public Rank Rank { get; }
        public Suit? Suit { get; }

        internal RepresentedCard(Rank rank, Suit? suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }

    public sealed class Combination
    {
        public CombinationKind Kind { get; }
        public IReadOnlyList<Card> Cards { get; }
        public IReadOnlyList<RepresentedCard> Represented { get; }
        public int Value { get; }

        internal Combination(CombinationKind kind, IReadOnlyList<Card> cards,
            IReadOnlyList<RepresentedCard> represented, int value)
        {
            Kind = kind;
            Cards = new List<Card>(cards).AsReadOnly();
            Represented = represented;
            Value = value;
        }
    }

    public static class Combinations
    {
        public static Combination Validate(IReadOnlyList<Card> cards, RunOrientation orientation)
        {
            if (cards == null || cards.Count < 3 || cards.Count > 5)
                return null;

            return TryMa7loul(cards, orientation) ?? TryMsaker(cards);
        }

        public static IReadOnlyList<IReadOnlyList<IReadOnlyList<Card>>> SplitRun(IReadOnlyList<Card> run)
        {
            if (run == null || run.Count < 6 || TryMa7loul(run, RunOrientation.Low) == null)
                throw new ArgumentException("SplitRun requires a same-suit ascending run of six or more cards.", nameof(run));

            var partitions = new List<IReadOnlyList<IReadOnlyList<Card>>>();
            Partition(run, 0, new List<IReadOnlyList<Card>>(), partitions);
            return partitions;
        }

        private static void Partition(IReadOnlyList<Card> run, int offset,
            List<IReadOnlyList<Card>> groups, List<IReadOnlyList<IReadOnlyList<Card>>> partitions)
        {
            int remaining = run.Count - offset;
            if (remaining == 0)
            {
                partitions.Add(groups.ToArray());
                return;
            }
            for (int size = 3; size <= 5 && size <= remaining; size++)
            {
                if (remaining - size != 0 && remaining - size < 3)
                    continue;
                var group = new List<Card>();
                for (int i = offset; i < offset + size; i++)
                    group.Add(run[i]);
                // a slice can be illegal even when the whole run is valid (e.g. all Jokers)
                if (Validate(group, RunOrientation.Low) == null)
                    continue;
                groups.Add(group.AsReadOnly());
                Partition(run, offset + size, groups, partitions);
                groups.RemoveAt(groups.Count - 1);
            }
        }

        private static readonly int[] HighRanks = { 1, 13, 12, 11, 10 };

        private static Combination TryMa7loul(IReadOnlyList<Card> cards, RunOrientation orientation)
        {
            Suit? suit = null;
            int start = 0;
            bool anchored = false;
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card.IsJoker)
                    continue;
                if (suit.HasValue && card.Suit.Value != suit.Value)
                    return null;
                suit = card.Suit.Value;
                if (!anchored)
                {
                    start = (int)card.Rank.Value - i;
                    anchored = true;
                }
            }
            if (!anchored)
                return null;
            if (orientation == RunOrientation.Low && (start < 1 || start + cards.Count - 1 > 13))
                return null;

            var represented = new List<RepresentedCard>();
            int value = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                var rank = orientation == RunOrientation.High ? (Rank)HighRanks[i] : (Rank)(start + i);
                if (!cards[i].IsJoker && cards[i].Rank.Value != rank)
                    return null;
                represented.Add(new RepresentedCard(rank, suit));
                value += orientation == RunOrientation.High ? 10 : Card.FaceValue(rank);
            }
            return new Combination(CombinationKind.Ma7loul, cards, represented, value);
        }

        private static Combination TryMsaker(IReadOnlyList<Card> cards)
        {
            if (cards.Count > 4)
                return null;

            Rank? rank = null;
            var suits = new HashSet<Suit>();
            foreach (var card in cards)
            {
                if (card.IsJoker)
                    continue;
                if ((rank.HasValue && card.Rank.Value != rank.Value) || !suits.Add(card.Suit.Value))
                    return null;
                rank = card.Rank.Value;
            }
            if (!rank.HasValue)
                return null;

            var missing = new List<Suit>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                if (!suits.Contains(suit))
                    missing.Add(suit);

            var represented = new List<RepresentedCard>();
            foreach (var card in cards)
                represented.Add(card.IsJoker
                    ? new RepresentedCard(rank.Value, missing.Count == 1 ? missing[0] : (Suit?)null)
                    : new RepresentedCard(card.Rank.Value, card.Suit));

            int value = cards.Count * (rank.Value == Rank.Ace ? 10 : Card.FaceValue(rank.Value));
            return new Combination(CombinationKind.Msaker, cards, represented, value);
        }
    }
}