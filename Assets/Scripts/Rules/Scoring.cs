using System.Collections.Generic;

namespace Rami.Rules
{
    public enum GameMode { Solo, TwoVsTwo }

    public static class Scoring
    {
        public static int RemainingPenalty(IReadOnlyList<Card> cards, GameMode mode)
        {
            int total = 0;
            foreach (var card in cards)
                total += card.IsJoker ? (mode == GameMode.Solo ? 10 : 20) : Card.FaceValue(card.Rank.Value);
            return total;
        }
    }
}