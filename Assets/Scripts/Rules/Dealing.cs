using System;
using System.Collections.Generic;

namespace Rami.Rules
{
    public enum DealStyle { War9a, Azwez, ThreeAdiya }

    public sealed class Cut
    {
        public IReadOnlyList<Card> First { get; }
        public IReadOnlyList<Card> Second { get; }
        public bool ChosenFirst { get; }

        internal Cut(IReadOnlyList<Card> first, IReadOnlyList<Card> second, bool chosenFirst)
        {
            First = first;
            Second = second;
            ChosenFirst = chosenFirst;
        }
    }

    public sealed class DealResult
    {
        private readonly List<Card>[] hands;
        private readonly List<Card> discardPile = new List<Card>();
        private bool openingDiscardDone;

        public int Dealer { get; }
        public int Cutter => (Dealer + 3) % 4;
        public IReadOnlyList<IReadOnlyList<Card>> Hands { get; }
        public IReadOnlyList<Card> Stock { get; }
        public Cut Cut { get; }
        public IReadOnlyList<Card> DiscardPile => discardPile;
        public int NextToAct { get; private set; }

        internal DealResult(int dealer, List<Card>[] hands, IReadOnlyList<Card> stock, Cut cut)
        {
            Dealer = dealer;
            this.hands = hands;
            Hands = Array.AsReadOnly(hands);
            Stock = stock;
            Cut = cut;
            NextToAct = dealer;
        }

        public void OpeningDiscard(int seat, Card card)
        {
            if (openingDiscardDone || seat != Dealer)
                throw new InvalidOperationException("Only the dealer may perform the opening discard, once.");
            if (!hands[seat].Remove(card))
                throw new InvalidOperationException("The opening discard must come from the dealer's hand.");
            discardPile.Add(card);
            NextToAct = (Dealer + 1) % 4;
            openingDiscardDone = true;
        }
    }

    public static class Dealing
    {
        private static readonly int[][] War9aPackets =
        {
            new[] { 1, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 0, 0, 0 },
        };

        private static readonly int[][] AzwezPackets =
        {
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 1, 0, 0, 0 },
        };

        private static readonly int[][] ThreeAdiyaPackets =
        {
            new[] { 3, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
            new[] { 2, 2, 2, 2 },
        };

        public static DealResult Deal(int dealer, DealStyle style, int seed)
        {
            if (dealer < 0 || dealer > 3)
                throw new ArgumentOutOfRangeException(nameof(dealer));

            var deck = new List<Card>(Deck.CreateDeck());
            var rng = new Random(seed);
            Deck.Shuffle(deck, rng);

            int largerSize = rng.Next(57, 108);
            bool chosenFirst = rng.Next(2) == 0;
            int firstSize = chosenFirst ? largerSize : deck.Count - largerSize;
            var first = deck.GetRange(0, firstSize);
            var second = deck.GetRange(firstSize, deck.Count - firstSize);
            var cut = new Cut(first, second, chosenFirst);

            var chosen = chosenFirst ? first : second;
            var other = chosenFirst ? second : first;

            var hands = new List<Card>[4];
            for (int i = 0; i < 4; i++)
                hands[i] = new List<Card>();
            int[][] packets;
            switch (style)
            {
                case DealStyle.War9a: packets = War9aPackets; break;
                case DealStyle.Azwez: packets = AzwezPackets; break;
                case DealStyle.ThreeAdiya: packets = ThreeAdiyaPackets; break;
                default: throw new ArgumentOutOfRangeException(nameof(style));
            }

            int taken = 0;
            foreach (var packet in packets)
                for (int i = 0; i < 4; i++)
                {
                    int seat = (dealer + i) % 4;
                    hands[seat].AddRange(chosen.GetRange(taken, packet[i]));
                    taken += packet[i];
                }

            var stock = new List<Card>(chosen.GetRange(taken, chosen.Count - taken));
            stock.AddRange(other);

            return new DealResult(dealer, hands, stock, cut);
        }
    }
}