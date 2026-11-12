using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class ScoringTests
    {
        private static Card C(Suit suit, Rank rank) => Card.Real(suit, rank, BackColour.Red);

        [Test]
        public void RemainingPenalty_RealCards_SumEndOfHandValues()
        {
            Assert.AreEqual(22, Scoring.RemainingPenalty(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.Two), C(Suit.Clubs, Rank.Jack)
            }, GameMode.Solo), "K + 2 + J");

            Assert.AreEqual(1, Scoring.RemainingPenalty(new[]
            {
                C(Suit.Hearts, Rank.Ace)
            }, GameMode.Solo), "uncombined 1 counts 1");

            Assert.AreEqual(1, Scoring.RemainingPenalty(new[]
            {
                C(Suit.Hearts, Rank.Ace)
            }, GameMode.TwoVsTwo), "uncombined 1 counts 1 in 2v2 too");

            Assert.AreEqual(15, Scoring.RemainingPenalty(new[]
            {
                C(Suit.Hearts, Rank.Five), C(Suit.Spades, Rank.Ten)
            }, GameMode.Solo), "number cards count face value");

            Assert.AreEqual(30, Scoring.RemainingPenalty(new[]
            {
                C(Suit.Hearts, Rank.Jack), C(Suit.Spades, Rank.Queen), C(Suit.Clubs, Rank.King)
            }, GameMode.Solo), "faces count 10 each");

            Assert.AreEqual(0, Scoring.RemainingPenalty(new Card[0], GameMode.Solo), "empty hand");
        }

        [Test]
        public void RemainingPenalty_Joker_Is10SoloAnd20TwoVsTwo()
        {
            var joker = Card.Joker(BackColour.Red);

            Assert.AreEqual(10, Scoring.RemainingPenalty(new[] { joker }, GameMode.Solo));
            Assert.AreEqual(20, Scoring.RemainingPenalty(new[] { joker }, GameMode.TwoVsTwo));

            Assert.AreEqual(20, Scoring.RemainingPenalty(new[]
            {
                joker, C(Suit.Hearts, Rank.King)
            }, GameMode.Solo), "Joker + K Solo");
            Assert.AreEqual(30, Scoring.RemainingPenalty(new[]
            {
                joker, C(Suit.Hearts, Rank.King)
            }, GameMode.TwoVsTwo), "Joker + K 2v2");

            Assert.AreEqual(20, Scoring.RemainingPenalty(new[]
            {
                joker, Card.Joker(BackColour.Blue)
            }, GameMode.Solo), "both back colours count the same");
        }
    }
}