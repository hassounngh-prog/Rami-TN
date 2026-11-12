using System;
using System.Collections.Generic;

namespace Rami.Rules
{
    public static class Deck
    {
        public static IReadOnlyList<Card> CreateDeck()
        {
            var cards = new List<Card>(108);
            for (int b = 0; b <= 1; b++)
            {
                var back = (BackColour)b;
                for (int s = 0; s < 4; s++)
                    for (int r = 1; r <= 13; r++)
                        cards.Add(Card.Real((Suit)s, (Rank)r, back));
                cards.Add(Card.Joker(back));
                cards.Add(Card.Joker(back));
            }
            return cards;
        }

        // One shuffle for the whole module: dealing cuts a fresh deck, an exhausted stock recycles
        // discards through the same seeded order.
        internal static void Shuffle(IList<Card> cards, Random rng)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var swap = cards[i];
                cards[i] = cards[j];
                cards[j] = swap;
            }
        }
    }
}