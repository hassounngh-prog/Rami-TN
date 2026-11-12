# Tunisian Rami glossary

Definitions below are drawn from Tunisian_Rami_Rules_v4.0.md and subsequent answers in this design discussion; the original rulebook has not been modified, and unconfirmed details remain open.

**Tunisian Rami**:
A card game for two to four players using 108 cards, in which players form valid combinations and try to empty their hand with a required final discard.
_Avoid_: Standard rummy rules assumed without confirmation.

**Solo**:
The mode in which every player competes individually against the other players.
_Avoid_: Single-player mode, offline play, or playing against a computer.

**2v2**:
The four-player mode with two teams of two and a combined score for each team.
_Avoid_: Two-player mode.

**Hand**:
One deal and the turns that follow until a player wins by emptying their hand after the required final discard.
_Avoid_: Match, which spans cumulative scoring; a player's hand also means the cards they currently hold.

**Match target**:
The selected cumulative penalty limit of 401, 601, or 801 points, whose attainment or crossing causes a player or team to lose.
_Avoid_: A score to reach to win.

**Scorekeeper**:
The designated player who records scores throughout the physical game and deals the first hand, with scorekeeping responsibility remaining fixed.
_Avoid_: Rotating dealer; a decision about who performs scorekeeping in the mobile app.

**ymachki**:
The pre-deal step in which the player on the dealer's left divides all 108 face-down cards into exactly two portions and chooses which portion to give the dealer first. The portion given to the dealer must hold at least 57 cards (owner decision, spec 0001), so the choice is forced to the larger portion.
_Avoid_: Revealing cards, dealing, or choosing a card from within the draw pile.

**war9a**:
The dealing style starting with one card for the dealer and two for each other player, followed by two-card tours and a final two cards for the dealer to finish with 15 while the others hold 14.
_Avoid_: One-card-at-a-time dealing throughout.

**azwez**:
The dealing style giving every player two cards per tour for seven tours, then one extra card to the dealer.
_Avoid_: A different final hand size from the other dealing styles.

**3adiya**:
The dealing style starting with three cards for the dealer and two for each other player, followed by six two-card tours to finish with 15 for the dealer and 14 for everyone else.
_Avoid_: An unspecified default dealing algorithm.

**tour**:
One complete pass of the dealing sequence through all players.
_Avoid_: A player's gameplay turn or an entire hand.

**msaker**:
A valid combination of three or four cards of the same rank using different suits, with Jokers permitted to represent missing cards under the Joker rules.
_Avoid_: Repeated suits within one combination or a requirement to match card-back colours.

**ma7loul**:
A valid combination of three to five consecutive ranks in one suit, allowing 1 at the low or high end but never a King–1–2 wraparound. The only Ace-high forms are 1–K–Q, 1–K–Q–J, and 1–K–Q–J–10 (bottom-to-top, 1 at the bottom), each counting 1 as 10; holding the 9 alongside 1–K–Q–J–10 forces a split rather than extending the high run (owner decision, feature 2 scope).
_Avoid_: Unlimited-length runs, treating 1 as both high and low within the same sequence, or any Ace-high form other than those three prefixes.

**Six-card split**:
Six consecutive cards of one suit must be divided into two valid three-card ma7loul combinations, including when an eligible place action adds a sixth card to a five-card table sequence.
_Avoid_: Keeping a six-card sequence as one table combination.

**Long-run split**:
A same-suit run of six or more cards is divided into valid three-to-five-card ma7loul combinations, and the player declares the partition (owner decision, feature 2 scope); six cards form two threes as in the six-card split. The partition never changes the run's total value.
_Avoid_: A forced canonical split, keeping any run over five as one combination, or a group smaller than three or larger than five.

**Joker**:
A substitute for a specific missing card in a valid msaker or ma7loul, valued as that represented card when included in a combination, with identical rules for red-backed and blue-backed Jokers.
_Avoid_: Applying the remaining-hand Joker penalty to a placed combination or inventing colour-specific Joker abilities.

**Joker limits**:
A msaker may use up to three Jokers and a ma7loul up to four, but every combination must contain at least one real non-Joker card and obey its normal size and validity rules.
_Avoid_: Joker-only combinations, a one-Joker limit, or exceeding the combination size.

**Bottom-to-top sequence order**:
The player chooses the valid arrangement, which determines Joker values: 6 below four Jokers scores 6 + 7 + 8 + 9 + 10 = 40, four Jokers below 6 score 2 + 3 + 4 + 5 + 6 = 20, and a natural 1–2–3 is displayed with 1 at the bottom, 2 above it, and 3 above 2. In an Ace-high ma7loul the 1 sits at the bottom, then K, Q, J, 10 going up (1–K–Q–J–10).
_Avoid_: Automatically choosing the highest-value interpretation or changing a Joker's represented rank after the combination has been dropped.

**Declared sequence**:
The player chooses a ma7loul's valid bottom-to-top order before dropping it, after which the represented ranks remain fixed while eligible exact replacements, extensions, and valid six-card splits are allowed.
_Avoid_: Reordering an already dropped 40-point sequence into a different 20-point sequence.

**Individual ma7loul Joker replacement**:
An eligible player may replace one Joker with its exact real card and reclaim it independently of any other Jokers in the same sequence.
_Avoid_: Requiring every Joker in that sequence to be replaced together.

**Multiple-Joker msaker reclamation**:
For one real King plus two Jokers, add a second distinct real King without taking a Joker to reach four cards before eligible individual replacements; for one real King plus three Jokers, an eligible player may directly exchange a second distinct real King for one Joker, keeping four cards and allowing subsequent individual replacements.
_Avoid_: Reclaiming all Jokers together as a requirement, allowing five cards, or changing the original two-real-cards-plus-one-Joker rule, which still requires both missing real suits to reclaim that Joker.

**Score-reduction drop**:
Laying down valid combinations in one turn whose combined value meets the current shared minimum, qualifying the player for a remaining-card penalty rather than the fixed 100-point loss penalty.
_Avoid_: Accumulating the qualifying total across several turns.

**Shared minimum**:
The qualifying drop threshold, initially selected as 51, 61, or 71 and raised after a qualifying drop to that drop's total value plus one for every player's next new-combination drop, including players who already dropped earlier in the hand. Each new hand resets it to the initially selected value; escalation lasts only for that hand (owner decision, feature 3a scope).
_Avoid_: A separate threshold for each player, an exemption for later small new-combination drops, the cumulative match target, or escalation carried over from one hand to the next.

**First-place eligibility**:
Unless the complete action wins immediately, a player who has not dropped must actually lay down qualifying combinations before performing ordinary place on existing table combinations.
_Avoid_: Treating qualifying combinations still held privately as sufficient permission to place.

**Discard pickup for a qualifying drop**:
A player may take the latest eligible discard and retain it while laying other combinations that meet the current minimum during that turn, so the picked-up card need not be part of the qualifying drop.
_Avoid_: Requiring the picked-up card to create or belong to the qualifying combinations, or allowing pickup without the qualifying drop or immediate-win condition.

**place**:
Adding cards from the active player's hand to any player's valid table combination during their own turn, subject to the documented eligibility rules.
_Avoid_: Taking a discard, acting out of turn, or assuming that placing a card alone ends the hand.

**Joker reclamation**:
A place action that takes a table Joker into the player's hand only when the current drop-point requirement is satisfied and the required real replacement cards are supplied, with an earlier drop or an otherwise possible immediate win insufficient to bypass that requirement.
_Avoid_: The superseded rulebook exception allowing Joker retrieval below the current minimum merely because the retrieved Joker would enable a win.

**Winning place without Joker retrieval**:
A player below the current drop minimum may finish by laying all their other valid combinations, placing the final unmatched real card into a table combination without taking its Joker, and discarding the last card, as in thirteen cards in combinations plus 6 placed into 5–Joker–7 plus the fifteenth card discarded.
_Avoid_: Taking or using the table Joker to complete the winning arrangement below the minimum.

**Ordinary place after dropping**:
A player who has actually made a qualifying drop may add cards to existing valid combinations on later turns without meeting the new minimum again, even when that addition does not win.
_Avoid_: Extending this permission to new-combination drops or Joker retrieval, which still require the current minimum.

**Immediate win**:
Finishing the entire winning arrangement through an eligible action and discarding the one final unmatched card so that no cards remain in hand.
_Avoid_: Merely completing one combination or placing before drawing.

**Stock / split**:
The face-down draw pile from which only the top card may be drawn, replenished when needed by shuffling older discards while leaving the latest discard face up.
_Avoid_: The pre-deal ymachki action or a pile from which any card may be chosen.

**Shared discard pile**:
The central face-up pile containing all players' discards, with only the immediately preceding player's latest discard eligible for a qualifying-drop or immediate-win pickup.
_Avoid_: Separate personal discard piles or permission to pick up older discards.

**Remaining-card penalty**:
The value of cards still held by a losing player who made a qualifying drop, using the rulebook's end-of-hand values rather than values those cards might have in a combination.
_Avoid_: The fixed 100-point penalty for a player who never made a qualifying drop.

**Private online table**:
The selected mobile experience in which friends play together from separate phones in a private game rather than through public matchmaking.
_Avoid_: Solo as a synonym for offline play, or an assumed requirement for computer opponents.

**First release**:
The initial mobile release for people who already know Tunisian Rami, including individual play for two to four players and four-player 2v2 at private online tables.
_Avoid_: A tutorial-first product or a release limited to only one of the two game modes.

**Automatic preparation**:
The app performs cutting and dealing automatically while preserving the agreed game rules and dealing styles.
_Avoid_: Requiring players to perform the physical preparation on-screen or silently changing the dealing rules.
