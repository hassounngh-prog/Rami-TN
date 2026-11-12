using System;
using System.Linq;
using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class OpeningDiscardTests
    {
        [Test]
        public void OpeningDiscard_DealerPlaysCardFromHand_PileStartsDealerLeft14NextSeatActs()
        {
            var result = Dealing.Deal(1, DealStyle.War9a, 555);
            Assert.AreEqual(1, result.NextToAct);
            var card = result.Hands[1][3];

            result.OpeningDiscard(1, card);

            Assert.AreEqual(14, result.Hands[1].Count);
            Assert.IsFalse(result.Hands[1].Contains(card));
            CollectionAssert.AreEqual(new[] { card }, result.DiscardPile);
            Assert.AreEqual(2, result.NextToAct);
        }

        [Test]
        public void OpeningDiscard_NonDealer_Rejected()
        {
            var result = Dealing.Deal(0, DealStyle.War9a, 555);
            var card = result.Hands[1][0];

            Assert.Throws<InvalidOperationException>(() => result.OpeningDiscard(1, card));
            Assert.AreEqual(15, result.Hands[0].Count);
            Assert.AreEqual(14, result.Hands[1].Count);
            Assert.IsEmpty(result.DiscardPile);
        }

        [Test]
        public void OpeningDiscard_SecondAttempt_Rejected()
        {
            var result = Dealing.Deal(0, DealStyle.War9a, 555);
            result.OpeningDiscard(0, result.Hands[0][0]);
            var second = result.Hands[0][0];

            Assert.Throws<InvalidOperationException>(() => result.OpeningDiscard(0, second));
            Assert.AreEqual(14, result.Hands[0].Count);
            Assert.AreEqual(1, result.DiscardPile.Count);
        }

        [Test]
        public void OpeningDiscard_CardNotInDealerHand_Rejected()
        {
            var result = Dealing.Deal(0, DealStyle.War9a, 555);
            var foreign = result.Stock.First(c => !c.IsJoker);

            Assert.Throws<InvalidOperationException>(() => result.OpeningDiscard(0, foreign));
            Assert.AreEqual(15, result.Hands[0].Count);
            Assert.IsEmpty(result.DiscardPile);
        }
    }
}