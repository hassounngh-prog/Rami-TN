using System;
using System.Collections.Generic;

namespace Rami.Rules
{
    public enum TurnProblem
    {
        None,
        NotYourTurn,
        MustTakeExtraCardFirst,
        AlreadyTookExtraCard,
        StockEmpty,
        StockNotEmpty,
        NotTheLatestDiscard,
        BelowMinimum,
        IllegalCombination,
        AlreadyDroppedThisTurn,
        CardNotInHand,
        CardUsedTwice
    }

    public sealed class TurnResult
    {
        public bool Accepted { get; }
        public TurnProblem Problem { get; }
        public Card Drawn { get; }

        private TurnResult(bool accepted, TurnProblem problem, Card drawn)
        {
            Accepted = accepted;
            Problem = problem;
            Drawn = drawn;
        }

        internal static TurnResult Ok()
        {
            return new TurnResult(true, TurnProblem.None, null);
        }

        internal static TurnResult Ok(Card drawn)
        {
            return new TurnResult(true, TurnProblem.None, drawn);
        }

        internal static TurnResult Fail(TurnProblem problem)
        {
            return new TurnResult(false, problem, null);
        }
    }

    public sealed class DeclaredCombination
    {
        public IReadOnlyList<Card> Cards { get; }
        public RunOrientation Orientation { get; }

        public DeclaredCombination(IReadOnlyList<Card> cards,
            RunOrientation orientation = RunOrientation.Low)
        {
            Cards = cards;
            Orientation = orientation;
        }
    }

    public sealed class TableCombination
    {
        public int Owner { get; }
        public Combination Combination { get; }

        internal TableCombination(int owner, Combination combination)
        {
            Owner = owner;
            Combination = combination;
        }
    }

    public sealed class SoloHand
    {
        private readonly List<Card>[] hands;
        private readonly List<Card> stock;
        private readonly List<Card> discards;
        private readonly List<TableCombination> table = new List<TableCombination>();
        private readonly bool[] dropped = new bool[4];
        private readonly Random rng;
        private bool extraCardTaken;
        private bool droppedThisTurn;

        public int ActiveSeat { get; private set; }
        public int CurrentMinimum { get; private set; }

        private SoloHand(List<Card>[] hands, List<Card> stock, List<Card> discards,
            int firstToAct, int currentMinimum, int seed)
        {
            this.hands = hands;
            this.stock = stock;
            this.discards = discards;
            rng = new Random(seed);
            ActiveSeat = firstToAct;
            CurrentMinimum = currentMinimum;
        }

        public static SoloHand Start(DealResult deal, int startingMinimum, int seed)
        {
            if (deal == null)
                throw new ArgumentNullException(nameof(deal));
            if (deal.Hands[deal.Dealer].Count != 14)
                throw new InvalidOperationException("The hand starts after the dealer's opening discard.");

            var hands = new List<Card>[4];
            for (int seat = 0; seat < 4; seat++)
                hands[seat] = new List<Card>(deal.Hands[seat]);

            return new SoloHand(hands, new List<Card>(deal.Stock), new List<Card>(deal.DiscardPile),
                deal.NextToAct, startingMinimum, seed);
        }

        public IReadOnlyList<Card> Hand(int seat) => hands[seat].AsReadOnly();

        public IReadOnlyList<Card> Discards => discards.AsReadOnly();

        public int StockCount => stock.Count;

        public Card LatestDiscard => discards.Count == 0 ? null : discards[discards.Count - 1];

        public bool HasDropped(int seat) => dropped[seat];

        public IReadOnlyList<TableCombination> Table => table.AsReadOnly();

        public TurnResult Drop(int seat, IReadOnlyList<DeclaredCombination> groups, Card pickup = null)
        {
            if (seat != ActiveSeat)
                return TurnResult.Fail(TurnProblem.NotYourTurn);
            if (droppedThisTurn)
                return TurnResult.Fail(TurnProblem.AlreadyDroppedThisTurn);
            if (pickup != null && extraCardTaken)
                return TurnResult.Fail(TurnProblem.AlreadyTookExtraCard);
            if (pickup != null && !pickup.Equals(LatestDiscard))
                return TurnResult.Fail(TurnProblem.NotTheLatestDiscard);
            if (pickup == null && !extraCardTaken)
                return TurnResult.Fail(TurnProblem.MustTakeExtraCardFirst);
            if (groups == null || groups.Count == 0)
                return TurnResult.Fail(TurnProblem.IllegalCombination);

            var held = new List<Card>(hands[seat]);
            if (pickup != null)
                held.Add(pickup);

            // Two same-back Jokers are indistinguishable by value, so a card is spendable as many
            // times as the hand actually holds a copy of it.
            var copies = new Dictionary<Card, int>();
            foreach (var card in held)
                copies[card] = copies.TryGetValue(card, out int alreadyHeld) ? alreadyHeld + 1 : 1;
            var spent = new Dictionary<Card, int>();

            var resolved = new List<Combination>();
            var used = new List<Card>();
            int total = 0;
            foreach (var group in groups)
            {
                var combination = Combinations.Validate(group.Cards, group.Orientation);
                if (combination == null)
                    return TurnResult.Fail(TurnProblem.IllegalCombination);
                foreach (var card in group.Cards)
                {
                    int copiesHeld = copies.TryGetValue(card, out int count) ? count : 0;
                    int wanted = (spent.TryGetValue(card, out int alreadySpent) ? alreadySpent : 0) + 1;
                    if (copiesHeld == 0)
                        return TurnResult.Fail(TurnProblem.CardNotInHand);
                    if (wanted > copiesHeld)
                        return TurnResult.Fail(TurnProblem.CardUsedTwice);
                    spent[card] = wanted;
                    used.Add(card);
                }
                total += combination.Value;
                resolved.Add(combination);
            }
            if (total < CurrentMinimum)
                return TurnResult.Fail(TurnProblem.BelowMinimum);

            if (pickup != null)
            {
                discards.RemoveAt(discards.Count - 1);
                hands[seat].Add(pickup);
                extraCardTaken = true;
            }
            foreach (var card in used)
                hands[seat].Remove(card);
            foreach (var combination in resolved)
                table.Add(new TableCombination(seat, combination));
            dropped[seat] = true;
            droppedThisTurn = true;
            CurrentMinimum = total + 1;
            return TurnResult.Ok();
        }

        public TurnResult Draw(int seat)
        {
            if (seat != ActiveSeat)
                return TurnResult.Fail(TurnProblem.NotYourTurn);
            if (extraCardTaken)
                return TurnResult.Fail(TurnProblem.AlreadyTookExtraCard);
            if (stock.Count == 0)
                return TurnResult.Fail(TurnProblem.StockEmpty);

            var card = stock[0];
            stock.RemoveAt(0);
            hands[seat].Add(card);
            extraCardTaken = true;
            return TurnResult.Ok(card);
        }

        // The whole centre except the latest discard goes back into the stock, which is then
        // drawn from like any other turn.
        public TurnResult RecycleAndDraw(int seat)
        {
            if (seat != ActiveSeat)
                return TurnResult.Fail(TurnProblem.NotYourTurn);
            if (extraCardTaken)
                return TurnResult.Fail(TurnProblem.AlreadyTookExtraCard);
            if (stock.Count > 0)
                return TurnResult.Fail(TurnProblem.StockNotEmpty);
            if (discards.Count < 2)
                throw new InvalidOperationException(
                    "A seat that exhausted the stock always left discards behind.");

            var latest = discards[discards.Count - 1];
            var older = discards.GetRange(0, discards.Count - 1);
            discards.Clear();
            discards.Add(latest);
            Deck.Shuffle(older, rng);
            stock.AddRange(older);
            return Draw(seat);
        }

        public TurnResult Discard(int seat, Card card)
        {
            if (seat != ActiveSeat)
                return TurnResult.Fail(TurnProblem.NotYourTurn);
            if (!extraCardTaken)
                return TurnResult.Fail(TurnProblem.MustTakeExtraCardFirst);
            if (!hands[seat].Remove(card))
                return TurnResult.Fail(TurnProblem.CardNotInHand);

            discards.Add(card);
            ActiveSeat = (seat + 1) % 4;
            extraCardTaken = false;
            droppedThisTurn = false;
            return TurnResult.Ok();
        }
    }
}
