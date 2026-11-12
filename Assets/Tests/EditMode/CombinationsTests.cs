using System;
using System.Linq;
using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class CombinationsTests
    {
        private static Card C(Suit suit, Rank rank) => Card.Real(suit, rank, BackColour.Red);

        private static Card J => Card.Joker(BackColour.Blue);

        [Test]
        public void Validate_JokerSubstitution_ReportsRepresentedCards()
        {
            var run = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Five), J, C(Suit.Hearts, Rank.Seven)
            }, RunOrientation.Low);
            Assert.IsNotNull(run);
            Assert.AreEqual(CombinationKind.Ma7loul, run.Kind);
            Assert.AreEqual(Rank.Six, run.Represented[1].Rank);
            Assert.AreEqual(Suit.Hearts, run.Represented[1].Suit, "ma7loul Joker is an exact rank+suit");

            var msaker = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), J
            }, RunOrientation.Low);
            Assert.IsNotNull(msaker);
            Assert.AreEqual(CombinationKind.Msaker, msaker.Kind);
            Assert.AreEqual(Rank.King, msaker.Represented[2].Rank);
            Assert.IsNull(msaker.Represented[2].Suit, "lone Joker with two missing suits is not pinned");

            var pinned = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), C(Suit.Clubs, Rank.King), J
            }, RunOrientation.Low);
            Assert.IsNotNull(pinned);
            Assert.AreEqual(Rank.King, pinned.Represented[3].Rank);
            Assert.AreEqual(Suit.Diamonds, pinned.Represented[3].Suit, "single missing suit pins the Joker");
        }

        [Test]
        public void Validate_JokerLimits_AtLimits_Accepts()
        {
            var msakerThreeJokers = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), J, J, J
            }, RunOrientation.Low);
            Assert.IsNotNull(msakerThreeJokers, "one real + three Jokers msaker");
            Assert.AreEqual(CombinationKind.Msaker, msakerThreeJokers.Kind);

            var runFourJokers = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), J, J, J, J
            }, RunOrientation.Low);
            Assert.IsNotNull(runFourJokers, "one real + four Jokers ma7loul");
            Assert.AreEqual(CombinationKind.Ma7loul, runFourJokers.Kind);
        }

        [Test]
        public void Validate_JokerLimits_JokerOnlyOrOverLimit_Rejects()
        {
            Assert.IsNull(Combinations.Validate(new[] { J, J, J }, RunOrientation.Low), "Joker-only group");
            Assert.IsNull(Combinations.Validate(new[] { J, J, J, J, J }, RunOrientation.High), "Joker-only five");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), J, J, J, J
            }, RunOrientation.Low), "msaker may not hold four Jokers");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Six), J, J, J, J, J
            }, RunOrientation.Low), "ma7loul may not hold five Jokers");
        }

        [Test]
        public void Validate_Msaker_ThreeOrFourSameRankDistinctSuits_Accepts()
        {
            var three = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), C(Suit.Clubs, Rank.King)
            }, RunOrientation.Low);
            Assert.IsNotNull(three);
            Assert.AreEqual(CombinationKind.Msaker, three.Kind);

            var four = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King),
                C(Suit.Clubs, Rank.King), C(Suit.Diamonds, Rank.King)
            }, RunOrientation.Low);
            Assert.IsNotNull(four);
            Assert.AreEqual(CombinationKind.Msaker, four.Kind);
        }

        [Test]
        public void Validate_Msaker_MixedBackColours_Accepts()
        {
            var combo = Combinations.Validate(new[]
            {
                Card.Real(Suit.Hearts, Rank.King, BackColour.Red),
                Card.Real(Suit.Spades, Rank.King, BackColour.Blue),
                Card.Real(Suit.Clubs, Rank.King, BackColour.Red)
            }, RunOrientation.Low);
            Assert.IsNotNull(combo);
            Assert.AreEqual(CombinationKind.Msaker, combo.Kind);
        }

        [Test]
        public void Validate_Ma7loulLow_ThreeToFiveConsecutiveSameSuit_Accepts()
        {
            var three = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three)
            }, RunOrientation.Low);
            Assert.IsNotNull(three);
            Assert.AreEqual(CombinationKind.Ma7loul, three.Kind);

            var five = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Six), C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Eight),
                C(Suit.Hearts, Rank.Nine), C(Suit.Hearts, Rank.Ten)
            }, RunOrientation.Low);
            Assert.IsNotNull(five);
            Assert.AreEqual(CombinationKind.Ma7loul, five.Kind);

            var tenToKing = Combinations.Validate(new[]
            {
                C(Suit.Spades, Rank.Ten), C(Suit.Spades, Rank.Jack),
                C(Suit.Spades, Rank.Queen), C(Suit.Spades, Rank.King)
            }, RunOrientation.Low);
            Assert.IsNotNull(tenToKing);
            Assert.AreEqual(CombinationKind.Ma7loul, tenToKing.Kind);
        }

        [Test]
        public void Validate_Ma7loulLow_GapsMixedSuitsWrongSizeWrap_Rejects()
        {
            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Four), C(Suit.Hearts, Rank.Five)
            }, RunOrientation.Low), "non-consecutive ranks");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three), C(Suit.Spades, Rank.Four)
            }, RunOrientation.Low), "mixed suits");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three)
            }, RunOrientation.Low), "fewer than 3");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three), C(Suit.Hearts, Rank.Four),
                C(Suit.Hearts, Rank.Five), C(Suit.Hearts, Rank.Six), C(Suit.Hearts, Rank.Seven)
            }, RunOrientation.Low), "more than 5");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.Two)
            }, RunOrientation.Low), "K-1-2 wraparound");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Three), C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Ace)
            }, RunOrientation.Low), "descending order is not bottom-to-top");
        }

        [Test]
        public void Validate_Ma7loulHigh_OnlyTheThreeAceHighForms_Accepts()
        {
            var three = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen)
            }, RunOrientation.High);
            Assert.IsNotNull(three, "1-K-Q");
            Assert.AreEqual(CombinationKind.Ma7loul, three.Kind);

            var four = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King),
                C(Suit.Hearts, Rank.Queen), C(Suit.Hearts, Rank.Jack)
            }, RunOrientation.High);
            Assert.IsNotNull(four, "1-K-Q-J");

            var five = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen),
                C(Suit.Hearts, Rank.Jack), C(Suit.Hearts, Rank.Ten)
            }, RunOrientation.High);
            Assert.IsNotNull(five, "1-K-Q-J-10");
        }

        [Test]
        public void Validate_Ma7loulHigh_WrongFormsAndOrientation_Rejects()
        {
            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen), C(Suit.Hearts, Rank.Jack)
            }, RunOrientation.High), "K-Q-J without the 1 at the bottom");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King),
                C(Suit.Hearts, Rank.Queen), C(Suit.Hearts, Rank.Ten)
            }, RunOrientation.High), "1-K-Q-10 skips the Jack");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen),
                C(Suit.Hearts, Rank.Jack), C(Suit.Hearts, Rank.Nine)
            }, RunOrientation.High), "1-K-Q-J-9");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen),
                C(Suit.Hearts, Rank.Jack), C(Suit.Hearts, Rank.Ten), C(Suit.Hearts, Rank.Nine)
            }, RunOrientation.High), "six-card high run holding the 9 must split");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Spades, Rank.King), C(Suit.Hearts, Rank.Queen)
            }, RunOrientation.High), "mixed suits");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three)
            }, RunOrientation.High), "low cards declared high");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen)
            }, RunOrientation.Low), "high cards declared low");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.Two)
            }, RunOrientation.High), "1 never both high and low");
        }

        [Test]
        public void Validate_RealCardCombinationValues_MatchRulebookExamples()
        {
            var high = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen)
            }, RunOrientation.High);
            Assert.AreEqual(30, high.Value, "1-K-Q high");

            var fourAces = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Spades, Rank.Ace),
                C(Suit.Diamonds, Rank.Ace), C(Suit.Clubs, Rank.Ace)
            }, RunOrientation.Low);
            Assert.AreEqual(40, fourAces.Value, "msaker of four 1s");

            var threeAces = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Spades, Rank.Ace), C(Suit.Diamonds, Rank.Ace)
            }, RunOrientation.Low);
            Assert.AreEqual(30, threeAces.Value, "msaker of three 1s");

            var lowRun = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Hearts, Rank.Two),
                C(Suit.Hearts, Rank.Three), C(Suit.Hearts, Rank.Four)
            }, RunOrientation.Low);
            Assert.AreEqual(10, lowRun.Value, "1-2-3-4 low");

            var sixToTen = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Six), C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Eight),
                C(Suit.Hearts, Rank.Nine), C(Suit.Hearts, Rank.Ten)
            }, RunOrientation.Low);
            Assert.AreEqual(40, sixToTen.Value, "6-7-8-9-10 low");

            var kings = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), C(Suit.Clubs, Rank.King)
            }, RunOrientation.Low);
            Assert.AreEqual(30, kings.Value, "msaker of Kings");
        }

        [Test]
        public void Validate_DeclaredOrderAndOrientation_FixJokerRanksAndValue()
        {
            var sixAtBottom = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Six), J, J, J, J
            }, RunOrientation.Low);
            Assert.IsNotNull(sixAtBottom);
            Assert.AreEqual(40, sixAtBottom.Value, "6 below four Jokers is 6-7-8-9-10");
            Assert.AreEqual(Rank.Ten, sixAtBottom.Represented[4].Rank);

            var sixAtTop = Combinations.Validate(new[]
            {
                J, J, J, J, C(Suit.Hearts, Rank.Six)
            }, RunOrientation.Low);
            Assert.IsNotNull(sixAtTop);
            Assert.AreEqual(20, sixAtTop.Value, "four Jokers below 6 is 2-3-4-5-6");
            Assert.AreEqual(Rank.Two, sixAtTop.Represented[0].Rank);

            var aceLow = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), J, J
            }, RunOrientation.Low);
            Assert.IsNotNull(aceLow);
            Assert.AreEqual(6, aceLow.Value, "1 + two Jokers low is 1-2-3");

            var aceHigh = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), J, J
            }, RunOrientation.High);
            Assert.IsNotNull(aceHigh);
            Assert.AreEqual(30, aceHigh.Value, "1 + two Jokers high is 1-K-Q");
            Assert.AreEqual(Rank.King, aceHigh.Represented[1].Rank);
            Assert.AreEqual(Rank.Queen, aceHigh.Represented[2].Rank);

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Six), J
            }, RunOrientation.Low), "invalid declared order");

            Assert.IsNull(Combinations.Validate(new[]
            {
                J, J, J, J, C(Suit.Hearts, Rank.Six)
            }, RunOrientation.High), "orientation disagrees with the real 6");
        }

        [Test]
        public void Validate_Msaker_IgnoresOrderAndOrientation()
        {
            var combo = Combinations.Validate(new[]
            {
                C(Suit.Spades, Rank.King), J, C(Suit.Hearts, Rank.King)
            }, RunOrientation.High);
            Assert.IsNotNull(combo);
            Assert.AreEqual(CombinationKind.Msaker, combo.Kind);
            Assert.AreEqual(30, combo.Value);
        }

        [Test]
        public void Validate_JokerCombinationValues_UseRepresentedCardValue()
        {
            var aceMsaker = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), C(Suit.Spades, Rank.Ace), J
            }, RunOrientation.Low);
            Assert.AreEqual(30, aceMsaker.Value, "Joker in msaker of 1s counts 10");

            var jokerAsAceHigh = Combinations.Validate(new[]
            {
                J, C(Suit.Hearts, Rank.King), C(Suit.Hearts, Rank.Queen)
            }, RunOrientation.High);
            Assert.IsNotNull(jokerAsAceHigh);
            Assert.AreEqual(30, jokerAsAceHigh.Value, "Joker as the high 1 counts 10");
            Assert.AreEqual(Rank.Ace, jokerAsAceHigh.Represented[0].Rank);
            Assert.AreEqual(Suit.Hearts, jokerAsAceHigh.Represented[0].Suit);

            var faceJoker = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), J
            }, RunOrientation.Low);
            Assert.AreEqual(30, faceJoker.Value, "Joker as a King counts 10");

            var lowJokerAce = Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.Ace), J, C(Suit.Hearts, Rank.Three)
            }, RunOrientation.Low);
            Assert.AreEqual(6, lowJokerAce.Value, "1-2-3 with Joker 2 is 6");
        }

        [Test]
        public void SplitRun_SixCards_ForcesExactlyTwoThrees()
        {
            var run = new[]
            {
                C(Suit.Hearts, Rank.Four), C(Suit.Hearts, Rank.Five), C(Suit.Hearts, Rank.Six),
                C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Eight), C(Suit.Hearts, Rank.Nine)
            };

            var partitions = Combinations.SplitRun(run);

            Assert.AreEqual(1, partitions.Count, "six cards have only the forced two 3s");
            var groups = partitions[0];
            Assert.AreEqual(2, groups.Count);
            CollectionAssert.AreEqual(
                new[] { Rank.Four, Rank.Five, Rank.Six }, groups[0].Select(c => c.Rank.Value));
            CollectionAssert.AreEqual(
                new[] { Rank.Seven, Rank.Eight, Rank.Nine }, groups[1].Select(c => c.Rank.Value));
            foreach (var group in groups)
                Assert.IsNotNull(Combinations.Validate(group, RunOrientation.Low));
        }

        [Test]
        public void SplitRun_SevenCards_ReturnsThreePlusFourAndFourPlusThree()
        {
            var run = Enumerable.Range(2, 7).Select(r => C(Suit.Spades, (Rank)r)).ToArray();

            var partitions = Combinations.SplitRun(run);

            Assert.AreEqual(2, partitions.Count);
            Assert.AreEqual(3, partitions[0][0].Count);
            Assert.AreEqual(4, partitions[0][1].Count);
            Assert.AreEqual(4, partitions[1][0].Count);
            Assert.AreEqual(3, partitions[1][1].Count);
            foreach (var partition in partitions)
            {
                int total = 0;
                foreach (var group in partition)
                {
                    var combo = Combinations.Validate(group, RunOrientation.Low);
                    Assert.IsNotNull(combo, "every group passes Validate");
                    total += combo.Value;
                }
                Assert.AreEqual(35, total, "partition never changes the run's total (2..8 = 35)");
            }
        }

        [Test]
        public void SplitRun_NineCards_ReturnsAllLegalPartitions()
        {
            var run = Enumerable.Range(1, 9).Select(r => C(Suit.Clubs, (Rank)r)).ToArray();

            var partitions = Combinations.SplitRun(run);

            Assert.AreEqual(3, partitions.Count, "9 = 3+3+3, 4+5, 5+4");
            foreach (var partition in partitions)
            {
                int total = 0;
                foreach (var group in partition)
                {
                    Assert.IsTrue(group.Count >= 3 && group.Count <= 5);
                    var combo = Combinations.Validate(group, RunOrientation.Low);
                    Assert.IsNotNull(combo);
                    total += combo.Value;
                }
                Assert.AreEqual(45, total, "1..9 low = 45");
            }
        }

        [Test]
        public void SplitRun_NotASixPlusRun_Throws()
        {
            var five = Enumerable.Range(2, 5).Select(r => C(Suit.Hearts, (Rank)r)).ToArray();
            Assert.Throws<ArgumentException>(() => Combinations.SplitRun(five));

            var notARun = new[]
            {
                C(Suit.Hearts, Rank.Two), C(Suit.Hearts, Rank.Three), C(Suit.Spades, Rank.Four),
                C(Suit.Hearts, Rank.Five), C(Suit.Hearts, Rank.Six), C(Suit.Hearts, Rank.Seven)
            };
            Assert.Throws<ArgumentException>(() => Combinations.SplitRun(notARun));
        }

        [Test]
        public void SplitRun_JokerRun_NeverReturnsAGroupThatFailsValidate()
        {
            var run = new[]
            {
                J, J, J, C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Eight), C(Suit.Hearts, Rank.Nine)
            };

            var partitions = Combinations.SplitRun(run);

            foreach (var partition in partitions)
                foreach (var group in partition)
                    Assert.IsNotNull(Combinations.Validate(group, RunOrientation.Low),
                        "AC-8: every group in every returned partition must pass Validate");
            Assert.AreEqual(0, partitions.Count, "the forced two-3s split isolates J,J,J, so no legal partition exists");
        }

        [Test]
        public void SplitRun_JokerRun_KeepsPartitionsWithValidGroups()
        {
            var run = new[]
            {
                C(Suit.Spades, Rank.Two), C(Suit.Spades, Rank.Three), J,
                C(Suit.Spades, Rank.Five), C(Suit.Spades, Rank.Six),
                C(Suit.Spades, Rank.Seven), C(Suit.Spades, Rank.Eight)
            };

            var partitions = Combinations.SplitRun(run);

            Assert.AreEqual(2, partitions.Count, "Joker as the 4 keeps both 3+4 and 4+3 legal");
            foreach (var partition in partitions)
                foreach (var group in partition)
                    Assert.IsNotNull(Combinations.Validate(group, RunOrientation.Low));
        }

        [Test]
        public void SplitRun_JokerRun_PrunesOnlyTheInvalidPartition()
        {
            var run = new[]
            {
                J, J, J, C(Suit.Hearts, Rank.Five), C(Suit.Hearts, Rank.Six),
                C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Eight)
            };

            var partitions = Combinations.SplitRun(run);

            Assert.AreEqual(1, partitions.Count, "3+4 isolates J,J,J (invalid); only 4+3 survives");
            Assert.AreEqual(4, partitions[0][0].Count);
            Assert.AreEqual(3, partitions[0][1].Count);
            foreach (var group in partitions[0])
                Assert.IsNotNull(Combinations.Validate(group, RunOrientation.Low));
        }

        [Test]
        public void Validate_Msaker_WrongSizeRepeatedSuitMixedRanks_Rejects()
        {
            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King)
            }, RunOrientation.Low), "two cards");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.King), C(Suit.Clubs, Rank.King),
                C(Suit.Diamonds, Rank.King), Card.Real(Suit.Hearts, Rank.King, BackColour.Blue)
            }, RunOrientation.Low), "five cards");

            Assert.IsNull(Combinations.Validate(new[]
            {
                Card.Real(Suit.Hearts, Rank.King, BackColour.Red),
                Card.Real(Suit.Hearts, Rank.King, BackColour.Blue),
                C(Suit.Spades, Rank.King)
            }, RunOrientation.Low), "repeated suit across back colours");

            Assert.IsNull(Combinations.Validate(new[]
            {
                C(Suit.Hearts, Rank.King), C(Suit.Spades, Rank.Queen), C(Suit.Clubs, Rank.Jack)
            }, RunOrientation.Low), "mixed ranks");
        }
    }
}