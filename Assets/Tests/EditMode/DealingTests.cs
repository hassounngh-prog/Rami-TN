using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class DealingTests
    {
        [Test]
        public void Deal_SameDealerStyleSeed_ProducesIdenticalHandsStockAndCut()
        {
            var a = Dealing.Deal(0, DealStyle.War9a, 12345);
            var b = Dealing.Deal(0, DealStyle.War9a, 12345);

            for (int seat = 0; seat < 4; seat++)
                CollectionAssert.AreEqual(a.Hands[seat], b.Hands[seat], $"seat {seat}");
            CollectionAssert.AreEqual(a.Stock, b.Stock);
            CollectionAssert.AreEqual(a.Cut.First, b.Cut.First);
            CollectionAssert.AreEqual(a.Cut.Second, b.Cut.Second);
            Assert.AreEqual(a.Cut.ChosenFirst, b.Cut.ChosenFirst);
        }

        [Test]
        public void Deal_All108CardsAppearExactlyOnceAcrossHandsAndStock()
        {
            var result = Dealing.Deal(2, DealStyle.War9a, 999);

            var all = result.Hands.SelectMany(h => h).Concat(result.Stock).ToList();
            Assert.AreEqual(108, all.Count);
            CollectionAssert.AreEquivalent(Deck.CreateDeck(), all);
        }

        [Test]
        public void Deal_Cut_TwoNonEmptyPortions_ChosenIsLargerAndAtLeast57_CutterBeforeDealer()
        {
            foreach (int dealer in new[] { 0, 1, 2, 3 })
            foreach (int seed in new[] { 1, 42, 999, 12345, 777777 })
            {
                var result = Dealing.Deal(dealer, DealStyle.War9a, seed);
                var cut = result.Cut;

                Assert.Greater(cut.First.Count, 0, $"seed {seed}");
                Assert.Greater(cut.Second.Count, 0, $"seed {seed}");
                Assert.AreEqual(108, cut.First.Count + cut.Second.Count);
                CollectionAssert.AreEquivalent(Deck.CreateDeck(), cut.First.Concat(cut.Second));

                var chosen = cut.ChosenFirst ? cut.First : cut.Second;
                var other = cut.ChosenFirst ? cut.Second : cut.First;
                Assert.GreaterOrEqual(chosen.Count, 57, $"seed {seed}");
                Assert.Greater(chosen.Count, other.Count, $"seed {seed}");
                Assert.AreEqual((dealer + 3) % 4, result.Cutter, $"dealer {seed}");
            }
        }

        [Test]
        public void Deal_AllStyles_Dealer15Others14Stock51()
        {
            foreach (var style in new[] { DealStyle.War9a, DealStyle.Azwez, DealStyle.ThreeAdiya })
            foreach (int dealer in new[] { 0, 1, 2, 3 })
            {
                var result = Dealing.Deal(dealer, style, 2026);

                Assert.AreEqual(15, result.Hands[dealer].Count, $"{style} dealer {dealer}");
                for (int i = 1; i < 4; i++)
                    Assert.AreEqual(14, result.Hands[(dealer + i) % 4].Count, $"{style} dealer {dealer} seat +{i}");
                Assert.AreEqual(51, result.Stock.Count, $"{style} dealer {dealer}");
            }
        }

        [Test]
        public void Deal_War9a_PacketSequenceMatchesRulebook()
        {
            var packets = new[]
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
            foreach (int dealer in new[] { 0, 1, 2, 3 })
                AssertPackets(DealStyle.War9a, dealer, 31337, packets);
        }

        [Test]
        public void Deal_Azwez_PacketSequenceMatchesRulebook()
        {
            var packets = new[]
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
            foreach (int dealer in new[] { 0, 1, 2, 3 })
                AssertPackets(DealStyle.Azwez, dealer, 31337, packets);
        }

        [Test]
        public void Deal_ThreeAdiya_PacketSequenceMatchesRulebook()
        {
            var packets = new[]
            {
                new[] { 3, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
                new[] { 2, 2, 2, 2 },
            };
            foreach (int dealer in new[] { 0, 1, 2, 3 })
                AssertPackets(DealStyle.ThreeAdiya, dealer, 31337, packets);
        }

        private static void AssertPackets(DealStyle style, int dealer, int seed, int[][] packets)
        {
            var result = Dealing.Deal(dealer, style, seed);
            var source = result.Cut.ChosenFirst ? result.Cut.First : result.Cut.Second;
            var other = result.Cut.ChosenFirst ? result.Cut.Second : result.Cut.First;

            var expected = new List<Card>[4];
            for (int i = 0; i < 4; i++)
                expected[i] = new List<Card>();
            int pos = 0;
            foreach (var packet in packets)
                for (int i = 0; i < 4; i++)
                {
                    int seat = (dealer + i) % 4;
                    for (int c = 0; c < packet[i]; c++)
                        expected[seat].Add(source[pos++]);
                }

            Assert.AreEqual(57, pos, "packet table must deal exactly 57 cards");
            for (int seat = 0; seat < 4; seat++)
                CollectionAssert.AreEqual(expected[seat], result.Hands[seat], $"{style} seat {seat}");
            CollectionAssert.AreEqual(source.Skip(57).Concat(other), result.Stock, $"{style} stock order");
        }
    }
}