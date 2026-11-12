using System.Linq;
using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class DeckTests
    {
        [Test]
        public void CreateDeck_Has108Cards_52RealsAnd2JokersPerBack()
        {
            var deck = Deck.CreateDeck();

            Assert.AreEqual(108, deck.Count);

            foreach (var back in new[] { BackColour.Red, BackColour.Blue })
            {
                var reals = deck.Where(c => c.Back == back && !c.IsJoker).ToList();
                Assert.AreEqual(52, reals.Count, $"real cards with {back} back");
                Assert.IsTrue(reals.All(c => c.Suit.HasValue && c.Rank.HasValue));
                CollectionAssert.AreEquivalent(
                    new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades },
                    reals.Select(c => c.Suit.Value).Distinct());
                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
                        Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King
                    },
                    reals.Select(c => c.Rank.Value).Distinct());
                Assert.IsTrue(
                    reals.GroupBy(c => new { c.Suit, c.Rank }).All(g => g.Count() == 1),
                    "no duplicate real cards within a back colour");

                var jokers = deck.Where(c => c.Back == back && c.IsJoker).ToList();
                Assert.AreEqual(2, jokers.Count, $"jokers with {back} back");
                Assert.IsTrue(jokers.All(c => !c.Suit.HasValue && !c.Rank.HasValue));
            }
        }
    }
}