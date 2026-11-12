using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rami.Rules.Tests
{
    public class SoloHandTests
    {
        private static SoloHand StartHand(out DealResult deal, int dealer = 0, int seed = 555,
            int startingMinimum = 51)
        {
            deal = Dealing.Deal(dealer, DealStyle.ThreeAdiya, seed);
            deal.OpeningDiscard(dealer, deal.Hands[dealer][0]);
            return SoloHand.Start(deal, startingMinimum, seed);
        }

        private static int[] Sizes(SoloHand hand)
        {
            var sizes = new int[4];
            for (int seat = 0; seat < 4; seat++)
                sizes[seat] = hand.Hand(seat).Count;
            return sizes;
        }

        // A 60-point set that fits seat 1's padded hand in the fabricated fixtures below.
        private static DeclaredCombination[] KingsAndQueens =
        {
            Group(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs)),
            Group(OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds))
        };

        [Test]
        public void Start_AfterOpeningDiscard_ExposesTheHandStartState()
        {
            DealResult deal;
            var hand = StartHand(out deal, dealer: 2);

            Assert.AreEqual(new[] { 14, 14, 14, 14 }, Sizes(hand), "everyone holds 14");
            Assert.AreEqual(51, hand.StockCount, "the opening discard left the stock untouched");
            Assert.AreEqual(deal.DiscardPile[0], hand.LatestDiscard);
            Assert.AreEqual(3, hand.ActiveSeat, "(dealer + 1) % 4 acts first");
            Assert.AreEqual(51, hand.CurrentMinimum);
            for (int seat = 0; seat < 4; seat++)
                Assert.IsFalse(hand.HasDropped(seat), $"seat {seat} has not dropped");
        }

        [Test]
        public void Start_DealerStillHoldsFifteen_Throws()
        {
            var deal = Dealing.Deal(0, DealStyle.ThreeAdiya, 555);

            Assert.Throws<InvalidOperationException>(() => SoloHand.Start(deal, 51, 555),
                "the opening discard belongs to feature 1");
        }

        [Test]
        public void Draw_ActiveSeat_TakesOnlyTheStockTop()
        {
            DealResult deal;
            var hand = StartHand(out deal);
            var top = deal.Stock[0];

            var result = hand.Draw(1);

            Assert.IsTrue(result.Accepted);
            Assert.AreEqual(top, result.Drawn);
            Assert.AreEqual(15, hand.Hand(1).Count);
            Assert.AreEqual(50, hand.StockCount);
            Assert.AreEqual(14, hand.Hand(3).Count, "the draw filled only the active seat");
            CollectionAssert.AreEqual(new[] { deal.DiscardPile[0] }, hand.Discards,
                "drawing moves no discard");
        }

        [Test]
        public void Draw_TwiceInOneTurn_RejectedAndChangesNothing()
        {
            var hand = StartHand(out _);
            Assert.IsTrue(hand.Draw(1).Accepted);
            var stockBefore = hand.StockCount;

            var second = hand.Draw(1);

            Assert.AreEqual(TurnProblem.AlreadyTookExtraCard, second.Problem);
            Assert.IsFalse(second.Accepted);
            Assert.AreEqual(15, hand.Hand(1).Count);
            Assert.AreEqual(stockBefore, hand.StockCount, "a rejected draw keeps the stock");
        }

        [Test]
        public void EveryAction_ByANonActiveSeat_RejectedAsNotYourTurn()
        {
            var hand = StartHand(out _);
            var before = Snapshot(hand);

            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Draw(3).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Draw(0).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Discard(2, Card.Real(Suit.Hearts, Rank.Five, BackColour.Red)).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Drop(0, KingsAndQueens).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Drop(3, KingsAndQueens, hand.LatestDiscard).Problem,
                "a drop by the wrong seat never reaches the pickup rules");
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.RecycleAndDraw(2).Problem,
                "only the active seat may recycle the stock");
            Assert.AreEqual(new[] { 14, 14, 14, 14 }, Sizes(hand));
            Assert.AreEqual(51, hand.StockCount);
            Assert.AreEqual(1, hand.ActiveSeat, "a rejected action does not pass the turn");
            Assert.AreEqual(before, Snapshot(hand));
        }

        [Test]
        public void Rejections_LeaveTheStockContentsAndOrderIntact()
        {
            DealResult deal;
            var hand = StartHand(out deal);
            var untouched = SoloHand.Start(deal, 51, 555);
            var illegal = Group(C(Suit.Hearts, Rank.Two), C(Suit.Spades, Rank.Five), C(Suit.Clubs, Rank.King));

            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Draw(0).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Drop(0, KingsAndQueens).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.Discard(2, hand.Hand(2)[0]).Problem);
            Assert.AreEqual(TurnProblem.NotYourTurn, hand.RecycleAndDraw(3).Problem);
            Assert.AreEqual(TurnProblem.MustTakeExtraCardFirst, hand.Discard(1, hand.Hand(1)[0]).Problem);
            Assert.AreEqual(TurnProblem.MustTakeExtraCardFirst, hand.Drop(1, KingsAndQueens).Problem);
            Assert.AreEqual(TurnProblem.StockNotEmpty, hand.RecycleAndDraw(1).Problem);
            Assert.AreEqual(TurnProblem.NotTheLatestDiscard,
                hand.Drop(1, KingsAndQueens, deal.Stock[0]).Problem, "a stock card is not a discard at all");

            for (int turn = 0; turn < 6; turn++)
            {
                int seat = hand.ActiveSeat;
                Assert.AreEqual(deal.Stock[turn], hand.Draw(seat).Drawn, $"disturbed session, turn {turn}");
                if (turn == 0)
                {
                    Assert.AreEqual(TurnProblem.AlreadyTookExtraCard, hand.Draw(seat).Problem);
                    Assert.AreEqual(TurnProblem.IllegalCombination, hand.Drop(seat, new[] { illegal }).Problem);
                    Assert.AreEqual(TurnProblem.IllegalCombination,
                        hand.Drop(seat, new DeclaredCombination[0]).Problem);
                }
                Assert.IsTrue(hand.Discard(seat, hand.Hand(seat).Last()).Accepted);

                int other = untouched.ActiveSeat;
                Assert.AreEqual(deal.Stock[turn], untouched.Draw(other).Drawn, $"untouched session, turn {turn}");
                Assert.IsTrue(untouched.Discard(other, untouched.Hand(other).Last()).Accepted);
            }

            Assert.AreEqual(hand.StockCount, untouched.StockCount, "both sessions spent the same six cards");
            Assert.AreEqual(45, hand.StockCount);
        }

        [Test]
        public void Discard_WithoutTheTurnsExtraCard_Rejected()
        {
            var hand = StartHand(out _);

            Assert.AreEqual(TurnProblem.MustTakeExtraCardFirst, hand.Discard(1, hand.Hand(1)[0]).Problem);
            Assert.AreEqual(14, hand.Hand(1).Count);
            Assert.AreEqual(1, hand.ActiveSeat);
        }

        [Test]
        public void Discard_CardNotHeld_Rejected()
        {
            var hand = StartHand(out _);
            hand.Draw(1);
            var foreign = hand.Hand(3).First(c => !hand.Hand(1).Contains(c));

            Assert.AreEqual(TurnProblem.CardNotInHand, hand.Discard(1, foreign).Problem);
            Assert.AreEqual(15, hand.Hand(1).Count);
            Assert.AreEqual(1, hand.ActiveSeat);
        }

        [Test]
        public void Discard_AfterDrawing_ClosesTheTurnAndAdvancesSeats()
        {
            DealResult deal;
            var hand = StartHand(out deal, dealer: 0);
            var opening = deal.DiscardPile[0];
            var drawn = hand.Draw(1).Drawn;

            var result = hand.Discard(1, drawn);

            Assert.IsTrue(result.Accepted);
            Assert.AreEqual(14, hand.Hand(1).Count, "15 - 1 discard");
            Assert.AreEqual(2, hand.ActiveSeat, "play continues to the next seat");
            Assert.AreEqual(51, hand.CurrentMinimum, "discarding never changes the minimum");
            Assert.AreEqual(drawn, hand.LatestDiscard);
            CollectionAssert.AreEqual(new[] { opening, drawn }, hand.Discards);

            // the remaining three seats, then the cycle returns to seat 1
            for (int expected = 2; expected != 1; expected = (expected + 1) % 4)
            {
                Assert.AreEqual(expected, hand.ActiveSeat);
                Assert.IsTrue(hand.Draw(expected).Accepted);
                Assert.IsTrue(hand.Discard(expected, hand.Hand(expected)[0]).Accepted);
            }
            Assert.AreEqual(1, hand.ActiveSeat);
            Assert.AreEqual(new[] { 14, 14, 14, 14 }, Sizes(hand));
        }

        private static Card C(Suit suit, Rank rank)
        {
            return Card.Real(suit, rank, BackColour.Red);
        }

        private static Card[] OfRank(int rank, params Suit[] suits)
        {
            var cards = new Card[suits.Length];
            for (int i = 0; i < suits.Length; i++)
                cards[i] = C(suits[i], (Rank)rank);
            return cards;
        }

        private static Card[] RunOf(Suit suit, int lowRank, int count)
        {
            var cards = new Card[count];
            for (int i = 0; i < count; i++)
                cards[i] = C(suit, (Rank)(lowRank + i));
            return cards;
        }

        private static Card[] Join(params Card[][] parts)
        {
            return parts.SelectMany(part => part).ToArray();
        }

        private static DeclaredCombination Group(params Card[] cards)
        {
            return new DeclaredCombination(cards);
        }

        private static DeclaredCombination GroupHigh(params Card[] cards)
        {
            return new DeclaredCombination(cards, RunOrientation.High);
        }

        // Seat 1 gets a known red-backed hand; every other seat, the opening discard and the stock
        // get deterministic blue-backed cards that never take part in a combination. The stock is
        // deliberately small so a test can reach exhaustion in a few turns.
        private static SoloHand HandWith(Card[] seatOne, int startingMinimum = 51, int stockCount = 6,
            int seed = 555)
        {
            var blue = Deck.CreateDeck().Where(card => card.Back == BackColour.Blue).ToList();
            var stock = blue.GetRange(43, stockCount);
            var hands = new List<Card>[4]
            {
                new List<Card>(blue.GetRange(1, 14)),
                new List<Card>(seatOne),
                new List<Card>(blue.GetRange(15, 14)),
                new List<Card>(blue.GetRange(29, 14))
            };
            var opening = blue[0];
            hands[0].Add(opening);
            // The cut only has to exist: a DealResult carries one, and SoloHand never reads it.
            var deal = new DealResult(0, hands, stock,
                new Cut(blue.GetRange(0, 43), blue.GetRange(43, 11), true));
            deal.OpeningDiscard(0, opening);
            return SoloHand.Start(deal, startingMinimum, seed);
        }

        // The cards a drop will declare, padded out to a real 14-card hand with low red-backed
        // cards that the test never lays down.
        private static Card[] HeldBySeatOne(params Card[] required)
        {
            var hand = new List<Card>(required);
            foreach (var card in Deck.CreateDeck())
            {
                if (hand.Count == 14)
                    break;
                if (!card.IsJoker && !hand.Contains(card))
                    hand.Add(card);
            }
            Assert.AreEqual(14, hand.Count, "fixture hand");
            return hand.ToArray();
        }

        private static string Name(Card card)
        {
            return card.IsJoker
                ? "J" + (card.Back == BackColour.Red ? "r" : "b")
                : card.Suit.Value.ToString().First() + (int)card.Rank.Value
                  + (card.Back == BackColour.Red ? "r" : "b");
        }

        // AC-9 needs the centre compared card by card and in order, not just counted.
        private static string Snapshot(SoloHand hand)
        {
            var parts = new List<string>();
            for (int seat = 0; seat < 4; seat++)
                parts.Add(hand.Hand(seat).Count + (hand.HasDropped(seat) ? "d" : "-"));
            parts.Add("min" + hand.CurrentMinimum);
            parts.Add("stock" + hand.StockCount);
            parts.Add("discards[" + string.Join(",", hand.Discards.Select(Name)) + "]");
            parts.Add("table" + hand.Table.Count);
            parts.Add("active" + hand.ActiveSeat);
            return string.Join(" ", parts);
        }

        [Test]
        public void Drop_QualifyingSet_LeavesTheHandRaisesMinimumAndRecordsTable()
        {
            var dropping = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var hand = HandWith(HeldBySeatOne(dropping));
            hand.Draw(1);
            var stockLeft = hand.StockCount;

            var result = hand.Drop(1, KingsAndQueens);

            Assert.IsTrue(result.Accepted, result.Problem.ToString());
            Assert.AreEqual(9, hand.Hand(1).Count, "15 - 6 dropped");
            Assert.AreEqual(61, hand.CurrentMinimum, "60 dropped -> next minimum 61 for everyone");
            Assert.IsTrue(hand.HasDropped(1));
            Assert.IsFalse(hand.HasDropped(2), "dropping marks only the acting seat");
            Assert.AreEqual(stockLeft, hand.StockCount, "a drop draws nothing from the stock");
            Assert.AreEqual(1, hand.Discards.Count, "a drop discards nothing");

            Assert.AreEqual(2, hand.Table.Count);
            Assert.AreEqual(1, hand.Table[0].Owner);
            Assert.AreEqual(CombinationKind.Msaker, hand.Table[0].Combination.Kind);
            Assert.AreEqual(30, hand.Table[0].Combination.Value);
            CollectionAssert.AreEqual(dropping.Take(3), hand.Table[0].Combination.Cards,
                "declared order is kept");
            CollectionAssert.AreEqual(dropping.Skip(3), hand.Table[1].Combination.Cards);

            Assert.IsTrue(hand.Discard(1, hand.Hand(1)[0]).Accepted);
            Assert.AreEqual(8, hand.Hand(1).Count, "14 - 6 dropped");
        }

        [Test]
        public void Drop_ExactlyAtTheMinimum_QualifiesAndSetsTheNextToTotalPlusOne()
        {
            var tens = OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs);
            var run = RunOf(Suit.Hearts, 6, 3);
            var hand = HandWith(HeldBySeatOne(Join(tens, run)));
            hand.Draw(1);

            Assert.IsTrue(hand.Drop(1, new[] { Group(tens), Group(run) }).Accepted,
                "30 + 21 = 51 reaches the minimum exactly");
            Assert.AreEqual(52, hand.CurrentMinimum);
        }

        [Test]
        public void Drop_BelowTheStartingMinimum_RejectedAndChangesNothing()
        {
            var dropping = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var hand = HandWith(HeldBySeatOne(dropping), startingMinimum: 61);
            hand.Draw(1);
            var held = hand.Hand(1).ToArray();
            var before = Snapshot(hand);

            var result = hand.Drop(1, KingsAndQueens);

            Assert.AreEqual(TurnProblem.BelowMinimum, result.Problem, "60 short of 61");
            Assert.AreEqual(before, Snapshot(hand));
            CollectionAssert.AreEqual(held, hand.Hand(1), "the rejected drop keeps every card in order");
        }

        [Test]
        public void Drop_IllegalGroup_RejectedAndNothingIsLaidDown()
        {
            var hand = HandWith(HeldBySeatOne(Join(
                OfRank(13, Suit.Hearts, Suit.Spades), RunOf(Suit.Hearts, 5, 3))));
            hand.Draw(1);
            var before = Snapshot(hand);

            Assert.AreEqual(TurnProblem.IllegalCombination,
                hand.Drop(1, new[] { Group(OfRank(13, Suit.Hearts, Suit.Spades)) }).Problem,
                "a two-card msaker");
            Assert.AreEqual(TurnProblem.IllegalCombination,
                hand.Drop(1, new[] { Group(new[]
                {
                    C(Suit.Hearts, Rank.Five), C(Suit.Hearts, Rank.Seven), C(Suit.Hearts, Rank.Nine)
                }) }).Problem,
                "cards held, but not a run and not a msaker");
            Assert.AreEqual(before, Snapshot(hand));
        }

        [Test]
        public void Drop_CardsNotHeldOrUsedTwice_Rejected()
        {
            var hand = HandWith(HeldBySeatOne(Join(RunOf(Suit.Hearts, 5, 5))));
            hand.Draw(1);

            Assert.AreEqual(TurnProblem.CardNotInHand,
                hand.Drop(1, new[] { Group(RunOf(Suit.Diamonds, 5, 3)) }).Problem,
                "a sequence the seat does not hold");
            Assert.AreEqual(TurnProblem.CardUsedTwice,
                hand.Drop(1, new[] { Group(RunOf(Suit.Hearts, 5, 3)), Group(RunOf(Suit.Hearts, 7, 3)) }).Problem,
                "7♥ belongs to both groups");
            Assert.AreEqual(15, hand.Hand(1).Count);
            Assert.AreEqual(0, hand.Table.Count);
        }

        [Test]
        public void Drop_BeforeDrawing_Rejected()
        {
            var hand = HandWith(HeldBySeatOne(Join(
                OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds))));

            Assert.AreEqual(TurnProblem.MustTakeExtraCardFirst, hand.Drop(1, KingsAndQueens).Problem);
        }

        [Test]
        public void Drop_SecondSetInTheSameTurn_Rejected()
        {
            var extra = OfRank(9, Suit.Hearts, Suit.Spades, Suit.Clubs);
            var hand = HandWith(HeldBySeatOne(Join(
                OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds), extra)));
            hand.Draw(1);
            Assert.IsTrue(hand.Drop(1, KingsAndQueens).Accepted);

            Assert.AreEqual(TurnProblem.AlreadyDroppedThisTurn,
                hand.Drop(1, new[] { Group(extra) }).Problem,
                "one drop closes the question for this turn");
            Assert.AreEqual(61, hand.CurrentMinimum, "the rejected second drop did not escalate again");
        }

        [Test]
        public void Drop_MinimumIsSharedAndCannotAccumulateAcrossTurns()
        {
            var first = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var later = Join(OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs), RunOf(Suit.Hearts, 6, 3));
            var hand = HandWith(HeldBySeatOne(Join(first, later)));

            hand.Draw(1);
            Assert.IsTrue(hand.Drop(1, KingsAndQueens).Accepted, "60 at minimum 51");
            hand.Discard(1, hand.Hand(1).Last());

            for (int seat = 2; seat != 1; seat = (seat + 1) % 4)
            {
                Assert.IsTrue(hand.Draw(seat).Accepted);
                Assert.IsTrue(hand.Discard(seat, hand.Hand(seat)[0]).Accepted);
                Assert.AreEqual(61, hand.CurrentMinimum, "turns without a drop change nothing");
            }

            hand.Draw(1);
            Assert.AreEqual(TurnProblem.BelowMinimum,
                hand.Drop(1, new[] { Group(OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs)),
                    Group(RunOf(Suit.Hearts, 6, 3)) }).Problem,
                "51 is below the 61 the same player already pushed up");
        }

        [Test]
        public void Start_NewSessionWithTheSameSetup_ResetsMinimumAndDroppedFlags()
        {
            var dropping = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            Assert.AreEqual(71, HandWith(HeldBySeatOne(dropping), startingMinimum: 71).CurrentMinimum,
                "the setup value is honoured");

            var played = HandWith(HeldBySeatOne(dropping));
            played.Draw(1);
            Assert.IsTrue(played.Drop(1, KingsAndQueens).Accepted);
            Assert.AreEqual(61, played.CurrentMinimum, "escalated inside that hand");

            var fresh = HandWith(HeldBySeatOne(dropping));

            Assert.AreEqual(51, fresh.CurrentMinimum, "a new hand restarts at the chosen minimum");
            Assert.IsFalse(fresh.HasDropped(1));
            Assert.AreEqual(0, fresh.Table.Count);
        }

        [Test]
        public void Drop_RecordsJokerRepresentationsAndDeclaredOrientation()
        {
            var tens = OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs, Suit.Diamonds);
            var aceHigh = new[] { C(Suit.Spades, Rank.Ace), C(Suit.Spades, Rank.King), C(Suit.Spades, Rank.Queen) };
            var jokerRun = new[] { C(Suit.Hearts, Rank.Four), Card.Joker(BackColour.Red), C(Suit.Hearts, Rank.Six) };
            var hand = HandWith(HeldBySeatOne(Join(tens, aceHigh, jokerRun)));
            hand.Draw(1);

            Assert.IsTrue(hand.Drop(1, new[] { Group(tens), GroupHigh(aceHigh), Group(jokerRun) }).Accepted,
                "40 + 30 + 15 = 85");
            Assert.AreEqual(3, hand.Table.Count);
            Assert.AreEqual(86, hand.CurrentMinimum);

            var recorded = hand.Table[1].Combination;
            Assert.AreEqual(CombinationKind.Ma7loul, recorded.Kind);
            Assert.AreEqual(30, recorded.Value);
            CollectionAssert.AreEqual(aceHigh, recorded.Cards, "1 sits at the bottom");

            var jokered = hand.Table[2].Combination;
            Assert.AreEqual(15, jokered.Value);
            CollectionAssert.AreEqual(jokerRun, jokered.Cards);
            Assert.AreEqual(3, jokered.Represented.Count);
            Assert.AreEqual(Rank.Five, jokered.Represented[1].Rank);
            Assert.AreEqual(Suit.Hearts, jokered.Represented[1].Suit);
        }

        [Test]
        public void Drop_TwoSameBackJokersInDifferentGroups_IsATableLegalDrop()
        {
            var redJoker = Card.Joker(BackColour.Red);
            var tens = OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs, Suit.Diamonds);
            var heartsRun = new[] { C(Suit.Hearts, Rank.Four), redJoker, C(Suit.Hearts, Rank.Six) };
            var spadesRun = new[] { C(Suit.Spades, Rank.Nine), redJoker, C(Suit.Spades, Rank.Jack) };
            var hand = HandWith(HeldBySeatOne(Join(tens, heartsRun, spadesRun)));
            hand.Draw(1);

            var result = hand.Drop(1, new[] { Group(tens), Group(heartsRun), Group(spadesRun) });

            Assert.IsTrue(result.Accepted, result.Problem.ToString());
            Assert.AreEqual(85, hand.CurrentMinimum, "40 + 15 + 29 = 84");
            Assert.AreEqual(5, hand.Hand(1).Count, "15 - 10 dropped");
            Assert.AreEqual(15, hand.Table[1].Combination.Value);
            Assert.AreEqual(29, hand.Table[2].Combination.Value);
            Assert.AreEqual(Rank.Ten, hand.Table[2].Combination.Represented[1].Rank);
            Assert.AreEqual(Suit.Spades, hand.Table[2].Combination.Represented[1].Suit);
            Assert.IsFalse(hand.Hand(1).Any(card => card.IsJoker), "both red Jokers went down");
        }

        [Test]
        public void Drop_OneJokerDeclaredInTwoGroups_IsStillCardUsedTwice()
        {
            var redJoker = Card.Joker(BackColour.Red);
            var tens = OfRank(10, Suit.Hearts, Suit.Spades, Suit.Clubs, Suit.Diamonds);
            var heartsRun = new[] { C(Suit.Hearts, Rank.Four), redJoker, C(Suit.Hearts, Rank.Six) };
            var spadesRun = new[] { C(Suit.Spades, Rank.Nine), redJoker, C(Suit.Spades, Rank.Jack) };
            var hand = HandWith(HeldBySeatOne(Join(tens,
                new[] { C(Suit.Hearts, Rank.Four), C(Suit.Hearts, Rank.Six) },
                new[] { C(Suit.Spades, Rank.Nine), C(Suit.Spades, Rank.Jack) },
                new[] { redJoker })));
            hand.Draw(1);
            var before = Snapshot(hand);

            var result = hand.Drop(1, new[] { Group(tens), Group(heartsRun), Group(spadesRun) });

            Assert.AreEqual(TurnProblem.CardUsedTwice, result.Problem,
                "one Joker cannot stand in for two groups");
            Assert.AreEqual(before, Snapshot(hand), "the rejected drop changed nothing");
            Assert.AreEqual(0, hand.Table.Count);
            Assert.AreEqual(15, hand.Hand(1).Count);
        }

        [Test]
        public void Drop_WithTheLatestDiscard_ReplacesTheDrawAndStillRequiresTheDiscard()
        {
            var dropping = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var hand = HandWith(HeldBySeatOne(dropping));
            var picked = hand.LatestDiscard;

            var result = hand.Drop(1, KingsAndQueens, picked);

            Assert.IsTrue(result.Accepted, result.Problem.ToString());
            Assert.AreEqual(9, hand.Hand(1).Count, "14 + pickup - 6 dropped");
            Assert.IsTrue(hand.Hand(1).Contains(picked), "the picked-up card may stay in hand");
            Assert.IsNull(hand.LatestDiscard, "the centre is empty until this turn discards");
            Assert.AreEqual(1, hand.ActiveSeat, "a pickup does not close the turn");
            Assert.AreEqual(61, hand.CurrentMinimum);

            Assert.IsTrue(hand.Discard(1, picked).Accepted, "the picked-up card may be the discard");
            Assert.AreEqual(8, hand.Hand(1).Count, "14 - 6 dropped");
            Assert.AreEqual(picked, hand.LatestDiscard, "discarding refills the centre");
            Assert.AreEqual(2, hand.ActiveSeat);
        }

        [Test]
        public void Pickup_WithoutAQualifyingDropOrAfterDrawingOrNotLatest_RejectedAndChangesNothing()
        {
            var shortSet = OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs);
            var hand = HandWith(HeldBySeatOne(shortSet));
            var before = Snapshot(hand);

            Assert.AreEqual(TurnProblem.BelowMinimum,
                hand.Drop(1, new[] { Group(shortSet) }, hand.LatestDiscard).Problem, "30 short of 51");
            Assert.AreEqual(before, Snapshot(hand));
            Assert.IsNotNull(hand.LatestDiscard, "the rejected pickup left the card in the centre");

            var qualifying = Join(shortSet, OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var drawn = HandWith(HeldBySeatOne(qualifying));
            drawn.Draw(1);
            Assert.AreEqual(TurnProblem.AlreadyTookExtraCard,
                drawn.Drop(1, KingsAndQueens, drawn.LatestDiscard).Problem,
                "the turn already took its extra card from the stock");

            var foreign = Deck.CreateDeck().Where(card => card.Back == BackColour.Blue).ElementAt(7);
            var stale = HandWith(HeldBySeatOne(qualifying));
            Assert.AreNotEqual(stale.LatestDiscard, foreign);
            Assert.AreEqual(TurnProblem.NotTheLatestDiscard,
                stale.Drop(1, KingsAndQueens, foreign).Problem, "an older discard is never eligible");
            Assert.AreEqual(1, stale.Discards.Count, "no rejected pickup touched the centre");
            Assert.AreEqual(14, stale.Hand(1).Count);
        }

        [Test]
        public void Pickup_AnOlderDiscardInAFullCentre_RejectedAndChangesNothing()
        {
            var qualifying = Join(OfRank(13, Suit.Hearts, Suit.Spades, Suit.Clubs),
                OfRank(12, Suit.Hearts, Suit.Spades, Suit.Diamonds));
            var hand = HandWith(HeldBySeatOne(qualifying));
            var oldest = hand.LatestDiscard;
            for (int turn = 0; turn < 4; turn++)
            {
                int seat = hand.ActiveSeat;
                Assert.IsTrue(hand.Draw(seat).Accepted);
                Assert.IsTrue(hand.Discard(seat, hand.Hand(seat).Last()).Accepted,
                    "each turn throws the card it just drew, so seat 1 keeps its set");
            }

            Assert.AreEqual(1, hand.ActiveSeat, "the cycle came back to seat 1");
            Assert.AreEqual(5, hand.Discards.Count, "the opening discard plus four turns");
            Assert.AreNotEqual(oldest, hand.LatestDiscard, "the centre now holds older cards");
            Assert.IsTrue(hand.Discards.Contains(oldest), "the oldest discard is still face up in the pile");

            var centreBefore = hand.Discards.ToList();
            var result = hand.Drop(1, KingsAndQueens, oldest);

            Assert.AreEqual(TurnProblem.NotTheLatestDiscard, result.Problem,
                "only the player on the left can be picked up from");
            CollectionAssert.AreEqual(centreBefore, hand.Discards, "and in the same order");
            Assert.AreEqual(14, hand.Hand(1).Count, "nothing left or entered the hand");
            Assert.AreEqual(0, hand.Table.Count, "nothing was laid down");
            Assert.AreEqual(51, hand.CurrentMinimum, "the minimum stayed for the next player");
        }

        // Two turns, so both stock cards end up in the centre and the stock is empty.
        private static SoloHand HandWithEmptyStock(Card[] seatOne, out Card latest, int seed = 99)
        {
            var hand = HandWith(seatOne, 51, 2, seed);
            Assert.IsTrue(hand.Draw(1).Accepted);
            Assert.IsTrue(hand.Discard(1, hand.Hand(1)[0]).Accepted);
            Assert.IsTrue(hand.Draw(2).Accepted);
            Assert.IsTrue(hand.Discard(2, hand.Hand(2)[0]).Accepted);
            Assert.AreEqual(0, hand.StockCount);
            latest = hand.LatestDiscard;
            return hand;
        }

        [Test]
        public void RecycleAndDraw_StockEmpty_RebuildsTheStockFromOlderDiscardsOnly()
        {
            Card latest;
            var hand = HandWithEmptyStock(HeldBySeatOne(RunOf(Suit.Hearts, 5, 3)), out latest);
            var opening = hand.Discards[0];
            var fiveOfHearts = RunOf(Suit.Hearts, 5, 3)[0];

            Assert.AreEqual(TurnProblem.StockEmpty, hand.Draw(3).Problem);
            var recycled = hand.RecycleAndDraw(3);

            Assert.IsTrue(recycled.Accepted, recycled.Problem.ToString());
            Assert.AreEqual(15, hand.Hand(3).Count, "the recycled top is the turn's extra card");
            Assert.AreEqual(1, hand.StockCount, "two older discards recycled, one drawn");
            Assert.AreEqual(1, hand.Discards.Count, "only the latest discard stays face up");
            Assert.AreEqual(latest, hand.LatestDiscard, "the latest discard is still there to be taken");
            Assert.IsFalse(hand.Discards.Contains(recycled.Drawn));

            Assert.IsTrue(hand.Discard(3, hand.Hand(3)[0]).Accepted);
            CollectionAssert.AreEquivalent(new[] { opening, fiveOfHearts },
                new[] { recycled.Drawn, hand.Draw(0).Drawn },
                "the recycled stock holds exactly the older discards");
        }

        [Test]
        public void RecycleAndDraw_SameSeed_RecyclesInOrder()
        {
            var first = HandWithEmptyStock(HeldBySeatOne(RunOf(Suit.Hearts, 5, 3)), out _, seed: 4242);
            var second = HandWithEmptyStock(HeldBySeatOne(RunOf(Suit.Hearts, 5, 3)), out _, seed: 4242);

            Assert.AreEqual(first.RecycleAndDraw(3).Drawn, second.RecycleAndDraw(3).Drawn);
        }

        [Test]
        public void RecycleAndDraw_RealDealStockRunsOut_CentreHolds52And51OlderAreRecycled()
        {
            DealResult deal;
            var hand = StartHand(out deal);
            while (hand.StockCount > 0)
            {
                int actor = hand.ActiveSeat;
                Assert.IsTrue(hand.Draw(actor).Accepted);
                Assert.IsTrue(hand.Discard(actor, hand.Hand(actor)[0]).Accepted);
            }

            Assert.AreEqual(52, hand.Discards.Count, "the opening discard plus one per turn");
            var actorSeat = hand.ActiveSeat;
            var latest = hand.LatestDiscard;
            var recycled = hand.RecycleAndDraw(actorSeat);

            Assert.IsTrue(recycled.Accepted, recycled.Problem.ToString());
            Assert.AreEqual(50, hand.StockCount, "51 older discards became the stock, then its top was drawn");
            Assert.AreEqual(15, hand.Hand(actorSeat).Count);
            Assert.AreEqual(1, hand.Discards.Count, "only the latest discard stays face up");
            Assert.AreEqual(latest, hand.LatestDiscard, "the next player may still take it");
            Assert.IsFalse(hand.Discards.Contains(recycled.Drawn), "the latest discard was never recycled");
        }

        [Test]
        public void RecycleAndDraw_StockStillThereOrTurnAlreadyDrawn_Rejected()
        {
            var hand = HandWith(HeldBySeatOne(RunOf(Suit.Hearts, 5, 3)), stockCount: 2);

            Assert.AreEqual(TurnProblem.StockNotEmpty, hand.RecycleAndDraw(1).Problem);
            Assert.AreEqual(2, hand.StockCount);

            Assert.IsTrue(hand.Draw(1).Accepted);
            Assert.AreEqual(TurnProblem.AlreadyTookExtraCard, hand.RecycleAndDraw(1).Problem);
            Assert.AreEqual(1, hand.Discards.Count, "the centre did not move");
        }

        [Test]
        public void RecycleAndDraw_NothingOlderToRecycle_ThrowsBecauseThatStateCannotHappen()
        {
            var hand = HandWith(HeldBySeatOne(RunOf(Suit.Hearts, 5, 3)), stockCount: 0);

            Assert.Throws<InvalidOperationException>(() => hand.RecycleAndDraw(1));
        }
    }
}
