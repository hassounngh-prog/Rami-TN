# Tunisian Rami — Rules and Game Specification

> **Status:** Working draft  
> **Version:** 4.0  
> **Last updated:** 12 November 2026  
> **Purpose:** Record the rules of the Tunisian Rami game as explained by the game owner. This document is intentionally explicit about unresolved details. Do not treat an open question or an assumption as an official rule.

## 1. Game overview

Tunisian Rami is a card game for **2 to 4 players**. Players build card combinations, draw and discard cards, and try to be the first to get rid of all their cards through valid combinations and permitted plays on combinations already placed on the table.

The game can be played in two modes:

- **Solo:** Every player competes against every other player.
- **2v2:** The four players form two teams of two.

The selected game target is one of:

- 401 points
- 601 points
- 801 points

**Match-loss rule:** The first player in Solo mode, or the first team in 2v2 mode, whose cumulative score reaches or passes the selected target loses the match. In Solo mode, if two or three players reach or pass the target in the same hand's score update, all of those players lose. In 2v2, the target is checked against the combined score of each team.

## 2. Cards and deck

The game uses **108 cards**:

- One 52-card deck with a **red back**.
- One 52-card deck with a **blue back**.
- Four Jokers: **2 red-backed Jokers** and **2 blue-backed Jokers**.

Each 52-card deck contains the four suits:

- Heart
- Spade
- Club
- Diamond

Each suit contains 13 ranks:

1. 1
2. 2
3. 3
4. 4
5. 5
6. 6
7. 7
8. 8
9. 9
10. 10
11. Jack
12. Queen
13. King

Therefore, each deck has `13 ranks × 4 suits = 52 cards`, and the two decks plus four Jokers make 108 cards.

The red/blue back colour distinguishes the two physical decks only. **The back colour does not restrict combinations:** cards in the same combination may have different back colours. For example, a `msaker` can contain cards with a mixture of red and blue backs, as long as the combination follows the suit/rank rules.

## 3. Players, scorekeeper, and dealing order

### 3.1 Scorekeeper and who starts

One designated player keeps the scores on paper throughout the game. The scorekeeping responsibility does **not rotate** between players after each hand. The first distribution is performed by this scorekeeper.

### 3.2 Splitting the cards — “ymachki”

Before the deal, the scorekeeper gives all **108 cards face down** to the player sitting immediately before the dealer (the player on the dealer's **left**). That player randomly separates the cards into **exactly two portions**, without revealing their faces. The player performing `ymachki` chooses which of the two portions to give to the dealer; the dealer uses that portion to begin the distribution. This pre-deal split is called **“ymachki.”** The cards remain face down throughout the process.

The player performing `ymachki`—not the dealer—chooses which portion is handed to the dealer.

### 3.3 Dealing direction

Cards are distributed from **right to left**, according to the dealer's seating order.

The dealer ends the deal with **15 cards**. Every other player receives **14 cards**.

For a game with `n` players, the dealer receives 15 cards and every other player receives 14 cards. The total number dealt is:

`15 + 14 × (n − 1)`

| Players | Cards dealt | Cards remaining in the stock/split |
|---:|---:|---:|
| 2 | 29 | 79 |
| 3 | 43 | 65 |
| 4 | 57 | 51 |

The undealt cards form the face-down stock/split from which players draw during play.

### 3.4 The three distribution styles

There are three named dealing styles:

#### A. “war9a”

The same dealing logic applies in games with **2, 3, or 4 players**; the dealer always finishes with 15 cards and every opponent with 14.

1. **First tour:** The dealer gives themselves **1 card**, then gives **2 cards** to each other player in turn, moving from right to left.
2. **Following tours:** The dealer gives **2 cards to themselves**, then gives **2 cards to each other player**, continuing from right to left. Repeat this two-card cycle for all players.
3. Continue until every other player has **14 cards**. At this point, the dealer has **13 cards**. With this method, this is reached at the end of tour 7.
4. **Final part:** The dealer gives themselves the last **2 cards**, reaching **15 cards**. The other players remain at 14 cards each.
5. The dealer looks at their 15 cards and discards **1 unwanted card** to begin play, leaving 14 cards. The next player then takes their turn.

#### B. “azwez”

The same dealing logic applies in games with **2, 3, or 4 players**; the dealer always finishes with 15 cards and every opponent with 14.

1. **First tour:** The dealer gives themselves **2 cards**, then gives **2 cards** to each other player in turn, moving from right to left.
2. **Following tours:** The dealer gives **2 cards to themselves** and **2 cards to each other player**, continuing from right to left. Repeat the same two-card cycle for all players.
3. At the end of **tour 7**, every player has **14 cards**.
4. **Final part:** The dealer gives themselves **1 additional card**, reaching **15 cards**. The other players remain with 14 cards each.

#### C. “3adiya” (normal)

The same dealing logic applies in games with **2, 3, or 4 players**; the dealer always finishes with 15 cards and every opponent with 14.

1. **First tour:** The dealer gives themselves **3 cards**, then gives **2 cards** to each other player in turn, moving from right to left.
2. **Following tours:** The dealer gives **2 cards to themselves** and **2 cards to each other player**, continuing from right to left. Repeat the same two-card cycle for all players.
3. At the end of **tour 7**, the dealer has **15 cards**, and every other player has **14 cards**.

## 4. Starting a hand and turn sequence

1. After the deal, the dealer has 15 cards and must discard one, leaving 14. Every other player starts with 14 cards.
2. At the start of a normal turn, the player has 14 cards and takes exactly one additional card, bringing the hand to 15. The extra card is taken either from the remaining stock/split (the undealt cards) or, when a special condition is met, from the discard made by the player immediately before them (the player on their left).
3. After taking a card, the player may make any permitted combinations/drop/place actions. They must then discard one card to finish the turn. If they have not laid down any cards, this returns their hand from 15 cards to 14. If they have laid down combinations, those cards are also removed from their hand.
4. Play continues to the next player in the established direction.

### 4.1 Shared discard pile

All players discard their cards to **one shared pile in the centre of the table**. There are not separate discard piles for individual players. The shared discard pile is kept **face up**, so all players can see the discarded cards. The stock/split is kept **face down**: the card faces are hidden and only the backs are visible. When drawing from the stock/split, a player may take **only the top card**; they cannot choose a card from deeper in the pile. The special pickup rule in Section 4 concerns only the card just discarded by the player immediately before the current player (the player on their left). Although all discards go into this shared pile and remain visible, older cards in the pile are not eligible for pickup.

**When the face-down stock/split runs out**, the current player has two possible choices concerning the latest card discarded by the player immediately before them:

1. **Take the latest discard** only if it allows the player to make a qualifying score-reduction drop or to complete their full winning hand immediately. If they take it for a qualifying score-reduction drop, they still complete their turn by discarding one card. If it completes their full winning hand, they discard their final unmatched card and the hand ends. In either case, the player does not need to draw from the stock, so the older discards are not recycled at that moment.
2. **Skip the latest discard and draw from a new stock.** The latest discarded card stays face up in the centre. All older cards in the central discard pile are collected, shuffled together, turned face down, and used to form the replenished stock/split. The player then draws only the top card of this new stock. For example, if the latest discard is `7♥` and it does not qualify for a special pickup, the player leaves `7♥` face up, shuffles the older discards into the new stock, and draws its top card. The `7♥` remains available as the latest discard for the next player, subject to the same special pickup conditions.

**Four-player card-count example:** After the deal, the four players hold 14 cards each (56 cards total), the dealer has made the opening discard, and the initial stock contains 51 cards. When all 51 stock cards have been drawn, the central discard pile contains 52 cards: the latest discard plus 51 older discards. Keep the latest card face up in the centre and shuffle the other 51 cards to make the new face-down stock. A discard pickup followed by a discard does not change the total number of cards in the central pile; it only replaces the latest card. Therefore, under this normal four-player flow, the discard pile is not reduced to a single card when the stock runs out.

The same card-count logic gives 66 central discards in a three-player game and 80 in a two-player game when the respective initial stock runs out. In every case, the newest discard stays face up and all older discards are recycled into the new stock when the player skips the newest card and needs to draw.

A player has only two ways to obtain the extra card for their turn:

1. Draw the **top card** from the remaining stock/split, which is kept face down; or
2. Take the **single card just discarded by the player immediately before them** (the player on their left), but only when one of the special conditions below is met.

The discard pickup is not a general choice. A player cannot take an older card from the central discard pile, nor a card discarded by another player earlier in the round. If the special condition is not met, the player must draw the top card from the face-down stock/split.

### 4.2 Actions are allowed only during the player's own turn

A player may perform game actions **only during their own turn**. No player may act out of turn. This applies to drawing or picking up a discard, laying down combinations for a score-reduction drop, placing cards onto combinations already on the table, reclaiming a Joker, and making the required final discard. A player may use the immediately preceding player's latest discard only when their own turn begins and the special pickup condition in Section 4.1 is satisfied.

Taking the immediately preceding player's discard is permitted only in either of these situations:

- **To win immediately:** the discarded card completes the player's full set of combinations. The player lays down all their combinations and discards one remaining card (their 15th card for that turn) to finish the hand.
- **To make a qualifying score-reduction drop:** the discarded card allows the player to lay down qualifying combination(s) whose total point value reaches at least the current minimum (51, 61, 71, or the raised threshold described in Section 8). The player must still discard one card to finish the turn.

**Core turn-count rule:** a player normally holds 14 cards before drawing, 15 after drawing, and must discard one card before the turn ends. This discard requirement also applies when the player takes the previous player's discard for a qualifying score-reduction drop; winning is the exception because it ends the hand.

## 5. Valid card combinations

There are two main combination families.

### 5.1 “msaker” — same-rank combinations

A **msaker** contains at least **3 cards of the same rank** (number or picture card), with each card having a **different suit**.

Examples of ranks include 1–10, Jack, Queen, and King.

- Minimum size: **3 cards**, using 3 different suits.
- Maximum size: **4 cards**, using all four suits.
- A three-card `msaker` is valid; a four-card `msaker` is also valid when the player has the fourth suit.
- Example of the minimum three-card `msaker`:
  - K♥ — Red back
  - K♠ — Blue back
  - K♣ — Red back
- Example of a four-card `msaker`:
  - K♥ — Red back
  - K♠ — Blue back
  - K♣ — Red back
  - K♦ — Blue back
- The required distinction is by **suit**, not back colour: Heart, Spade, Club, and Diamond must be different within the combination. The cards do **not** need to share the same back colour; red-backed and blue-backed cards can be mixed freely in one `msaker`.

### 5.2 “ma7loul” — consecutive sequences

A **ma7loul** is a sequence of consecutive ranks in the **same suit**.

The normal rank order is:

`1 → 2 → 3 → 4 → 5 → 6 → 7 → 8 → 9 → 10 → Jack → Queen → King`

Rules explained so far:

- A sequence must contain at least **3 consecutive cards**.
- A sequence can contain up to **5 consecutive cards**.
- If a player has a sequence of 6, they split it into two valid combinations of 3 cards each.
- A special sequence is allowed in descending order from **1 to King**:
  `1 → King → Queen → Jack → 10`
- This special sequence is limited to 5 cards. If the player also has the 9, they must split the cards rather than extend that special sequence.

**Sequence detail to confirm:** Whether the special `1–King–Queen–Jack–10` sequence is the only use of Ace/1 as a high card, and how overlapping sequences should be split, have not yet been specified. The six-card rule is confirmed: split six consecutive cards into two three-card combinations.

## 6. Jokers

A Joker can replace a missing card in **both combination types: `msaker` and `ma7loul`**.

Examples:

- **`ma7loul`:** A player has `1♥` and `3♥`. A Joker can represent `2♥` to complete `1♥–2♥–3♥`.
- **`msaker`:** A player has `K♥` and `K♠`. A Joker can represent a missing King of a different suit (for example, `K♣`) to complete a valid three-card `msaker`.

The Joker represents the specific missing card needed to make the combination valid. The usual combination requirements still apply: a `msaker` must use different suits, and a `ma7loul` must be consecutive cards of the same suit.

For scoring, a Joker is worth **the point value of the card it represents** in that combination. For example, if it replaces a 7, it counts as 7; if it replaces a face card, it counts as 10.

### 6.1 Reclaiming a Joker from a placed “msaker”

A player can reclaim a Joker from another player's `msaker` by completing that `msaker` with **both missing real cards of the same rank**. This turns the `msaker` into a four-card combination containing all four suits, and the player takes the Joker into their hand.

Example:

- The combination on the table is `K♥ + K♠ + Joker`.
- The Joker represents one of the missing Kings (for example, `K♣`).
- To reclaim the Joker, the player must have **both** missing real Kings: `K♣` and `K♦`.
- The player places both Kings onto the `msaker`, completing the four-suit King combination, and takes the Joker into their hand.

This Joker-reclaim action is not freely available on its own. The player must meet one of these conditions:

1. **Score-reduction eligibility:** The player can make a qualifying score-reduction drop under the current minimum.
2. **Full immediate-win exception:** If the player cannot meet the current minimum, they may still reclaim the Joker only if the whole action lets them complete their winning hand. “Win immediately” does **not** mean that placing the missing Kings alone is enough. The player must already have (or be able to form after the Joker is reclaimed) all the combinations needed to win, then discard their final unmatched card to finish the hand.

For example, the player may have four three-card combinations (`3 + 3 + 3 + 3` — these numbers describe combination sizes, not point totals), the two missing Kings needed for the `msaker` place, and one final unmatched card. They place both Kings to complete the four-suit King `msaker`, take the Joker, complete their full winning arrangement, and discard the final unmatched card. The important condition is that the entire hand can be finished—not merely that the table combination becomes complete.

### 6.2 Reclaiming a Joker from a placed “ma7loul”

A player can reclaim a Joker from another player's `ma7loul` by placing the **exact real card represented by the Joker** into the sequence. The player then takes the Joker into their hand.

Example:

- The combination on the table is `5♥ + Joker + 7♥`.
- The Joker represents `6♥`.
- The player places the real `6♥` into the sequence and takes the Joker into their hand.

The same two eligibility conditions used for reclaiming a Joker from a `msaker` apply:

1. **Score-reduction eligibility:** The player can make a qualifying score-reduction drop under the current minimum.
2. **Full immediate-win exception:** If the player cannot meet the current score-reduction minimum, they may reclaim the Joker only if placing the missing real card allows them to complete their entire winning hand. “Win immediately” does **not** mean that placing `6♥` by itself wins. The player must have all the other combinations needed to win and, after placing `6♥` and taking the Joker, be able to finish the full arrangement and discard their final unmatched card.

For example, the player may have combinations arranged as `3 + 3 + 4 + 3` (these numbers describe combination sizes, not point totals), the real `6♥` needed for the `ma7loul` place, and one final unmatched card. They place `6♥` into the opponent's `5♥ + Joker + 7♥` sequence, take the Joker, complete the full winning arrangement, and discard the final unmatched card. The important condition is that the whole hand can be finished—not merely that the sequence is extended.

This is not a free-standing Joker exchange. The card must complete the specific missing position represented by the Joker, and the player must satisfy one of the two eligibility conditions above.

The following Joker rule still needs confirmation:

- Whether the red-backed and blue-backed Jokers have any different rules.

## 7. Winning a hand

The first player who completes all their cards into valid combinations and gets rid of their final card wins the hand.

**Meaning of “win immediately” in special plays:** A special action (such as taking the immediately preceding player's discard or reclaiming a Joker from an opponent's combination) counts as an immediate win only when the player can complete the **entire winning hand arrangement** through that action. Placing one missing card onto an opponent's combination is not, by itself, a win. The player must be able to arrange all the other cards into valid combinations as required and then discard their one final unmatched card to finish the hand.

The described 14-card layouts include:

- `4 + 4 + 3 + 3 = 14`
- `5 + 3 + 3 + 3 = 14`

These are examples of how a player's 14 cards may be arranged into combinations. The player then discards their 15th/final card to finish, depending on the exact turn state.

### 7.1 Taking the previous player's discarded card to win

A player may take a card discarded by the player immediately before them (on their left) when it completes their final combination and allows them to win.

Example:

- The player holds `1♥`, `3♥`, and `5♥` as described in the explanation.
- The player before them discards `2♥`.
- The player can use `2♥` to complete `1♥–2♥–3♥`, then discard `5♥` as their final discard and win.

Taking a discarded card to win is one of the two special permitted discard-pickup cases; taking it for ordinary combination-building is not allowed. The other permitted case is when the discarded card makes a qualifying score-reduction drop possible (Section 8).

## 8. Placing combinations to reduce the loss

A player does not always need to wait until they can finish their entire hand. They can place valid combinations on the table to reduce the penalty they receive if another player wins. This is the **score-reduction drop** described by the players. The normal turn-count rule still applies: the player takes one card (going from 14 to 15), may lay down qualifying combinations, and must discard one remaining card to finish the turn. Cards laid down are no longer part of the player's hand; the player discards one additional card from the cards still held.

### 8.1 Minimum value required to drop

**Before the game starts, the players choose the starting score-reduction minimum:** **51, 61, or 71 points**. The selected value becomes the initial threshold for qualifying score-reduction drops in that game. The player must lay down combination(s) whose combined point value reaches at least the current minimum.

After play begins, the minimum can increase according to the rule in Section 8.3. The initial choice of 51, 61, or 71 is made before the game starts; it is not reselected each time a player drops.

### 8.2 Single-turn requirement

A player must meet the current minimum **in one turn**. They cannot lay down part of the required total in one turn and add more combinations in a later turn to reach the minimum. For example, if the current minimum is 71 and a player has combinations worth 30, 25, and 20, they must lay down all three together in the same turn (total 75) to qualify.

### 8.3 The minimum increases after a drop

After every qualifying drop, whether its total exactly meets or exceeds the current minimum, the minimum for the next qualifying drop becomes **one point higher than the value of the most recent qualifying drop**. This is **one shared minimum for the entire game hand**: it applies to every player, not just the player who made the latest drop. All players must meet the same current threshold for their next qualifying score-reduction drop.

Example:

- The starting minimum is 71.
- A player drops valid combinations with a total value of 80.
- The shared minimum for **every player's** next qualifying drop becomes 81.
- If any player next makes a qualifying drop worth exactly 81, the shared minimum for **all players** becomes 82.
- The same rule continues: if a later qualifying drop by any player is worth 90, the shared minimum for all players becomes 91.

In formula form:

`next minimum = most recent qualifying drop total + 1`

This is a changing, **global threshold**, not a permanent return to the original 51/61/71 value after each player. It is shared by all players and is not tracked separately for individual players.

### 8.4 Remaining-card penalty

The penalty depends on whether the losing player made a **qualifying score-reduction drop** during the hand:

- **No qualifying score-reduction drop:** the player receives a fixed **+100-point penalty**, regardless of the cards remaining in their hand.
- **Made a qualifying score-reduction drop:** the player's penalty is the total value of the uncombined cards remaining in their hand. Count those cards using Section 12.2.

For example, a player who made a qualifying drop and is left holding a King, a 2, and a Jack receives **22 points**: King = 10, 2 = 2, and Jack = 10. A remaining 1 counts as 1 point. A remaining Joker counts as 10 points in Solo mode or 20 points in 2v2 mode. The fixed 100-point penalty applies to a player who did not make a qualifying drop even if their actual remaining cards would total less or more than 100.

The increased minimum is shared by every player for the rest of the hand; there are no player-specific thresholds.

## 9. “Place” — adding cards to combinations already on the table

**Place** is the action of adding a card from a player's hand to a valid combination already on the table. A player may place onto **any player's combination**: the combination owner does not matter. In Solo mode, this includes any opponent's combination; in 2v2 mode, it includes both opponents' and the teammate's combinations. The player is not restricted to placing on the combination of the player immediately before them.

Example:

- An opponent has placed a Diamond sequence containing `Jack♦–Queen♦–King♦`.
- A player holds `10♦` as their final uncombined card.
- The player can add `10♦` to the sequence in the appropriate position and win.

A player may have more than one card available for **place**. Reclaiming a Joker from a `msaker` (Section 6.1) or a `ma7loul` (Section 6.2) is a special place action; the player must add the real card(s) represented by the Joker and meet the applicable eligibility condition.

**Winning by placing your final unmatched card:** A player does not win simply by placing their last unmatched card onto an existing combination before drawing. They must follow the normal turn sequence:

1. Start the turn with their remaining unmatched card in hand.
2. Draw one card, as required at the start of the turn.
3. Place the previously held unmatched card onto a valid combination already on the table.
4. Discard the card just drawn.
5. Their hand is now empty, so they win the hand.

For example, if a player has already laid down all their other combinations and has only `10♦` left in hand, they draw one card from the stock (or take an eligible discard under the normal special-pickup rules), place `10♦` onto a suitable Diamond sequence on the table, and discard the card they just drew. They win after that discard. The drawn card is not placed as the winning card in this example; it is the card discarded to finish the turn.

The player is allowed to perform place when at least one of these conditions applies:

- They have already placed their own combinations to reduce their loss.
- They are able to place combinations for loss reduction at that time.
- The card being placed is their **last card outside their own combinations** (their final uncombined card).

**Target and timing:** A player may place cards onto any valid combination on the table, regardless of which player or team originally laid it down. A player may perform `place` only during their own turn, in accordance with Section 4.2.

## 10. Scoring

### 10.1 Solo mode

In Solo mode, every player competes individually.

- The winner always receives **−10 points**, regardless of how they win the hand. This applies whether they win by completing their combinations normally, taking the immediately preceding player's discard, using `place`, or reclaiming a Joker under the permitted conditions.
- A losing player who did **not** make a qualifying score-reduction drop receives a fixed **+100-point penalty**, regardless of the cards remaining in their hand.
- A losing player who **did** make a qualifying score-reduction drop receives a penalty equal to the value of the uncombined cards remaining in their hand, as described in Section 8.4 and valued in Section 12.2.
- The first Solo player whose cumulative score reaches or passes the selected match target loses. If two or three players reach or pass the target in the same hand's score update, all of those players lose.

Example score record:

| Player | Points |
|---|---:|
| Hassoun | −10 |
| Bilel | 100 |
| Anoir | 48 |
| Salah | 10 |

This example is consistent with Hassoun winning for −10, Bilel receiving the fixed +100 penalty for not making a qualifying score-reduction drop, and Anoir and Salah receiving reduced penalties based on the values of their remaining uncombined cards after qualifying drops.

### 10.2 2v2 mode

In 2v2 mode, players form two teams of two. The losing team's penalty is calculated by **adding the penalties of both teammates**. Each player's penalty is determined separately according to whether that player made a qualifying score-reduction drop:

- A player who made a qualifying score-reduction drop receives a penalty equal to the total value of the uncombined cards remaining in their hand.
- A player who did not make a qualifying score-reduction drop receives the standard **100-point** penalty.
- The two individual penalties are then added to produce the losing team's total for the hand.

**Example:** One losing teammate made a score-reduction drop and has 14 points' worth of uncombined cards remaining. The other teammate did not make a score-reduction drop and receives the standard 100-point penalty. The losing team's total is:

`14 + 100 = 114 points`

If neither teammate made a score-reduction drop, the two standard penalties total `100 + 100 = 200 points`, matching the example team result of **+200**. The winning team always receives **−20 points** when either teammate wins, regardless of how that player wins the hand. Record this as the winning team's combined score for the hand; it does not change based on whether the win comes from completing combinations normally, taking a discard, using `place`, or reclaiming a Joker under the permitted conditions.

The losing team's score is therefore a combined team total; it is not calculated by choosing only one teammate's penalty. A losing teammate cannot contribute a **0-point** penalty: if a player had no cards left and had completed the required final discard, that player would have won and ended the hand. Therefore, a team-penalty example such as `0 + 100` is not possible.

For the match target, check the **combined cumulative score of the two teammates**. The first team whose combined score reaches or passes the selected target loses the match.

## 11. Dealer and cutter rotation between hands

After a player wins a hand:

1. The distribution moves to the next player on the **right** of the previous dealer.
2. The cutting/splitting role moves with the rotation: the player who previously handled the split is no longer the cutter; the new arrangement is described as the previous dealer handling the split for the next hand.
3. The new dealer receives the split and performs the next distribution.

The intended rotation is described through this example:

- If Player A is the dealer, the player on A's left performs the “ymachki” split.
- After the hand, the next dealer is the player to A's right.
- The split is then handled by A, and the new dealer distributes.

This rotation should be confirmed against the actual seating order before being treated as final.

## 12. Card point values

Card values are used to calculate both the total value of combinations for a score-reduction drop and the penalty from cards left in a player's hand. A combination's value is the sum of the values of the cards in that combination. The value of rank **1** depends on the type of combination in which it is used.

| Card | Point value | Rule / condition |
|---|---:|---|
| 1 in a low `ma7loul` sequence | 1 | When used as the low card in a sequence such as `1–2–3` or `1–2–3–4`. |
| 1 in the high `ma7loul` sequence | 10 | When used in the special high sequence beginning `1–King–Queen–Jack–10`; it is worth 10 in a valid part of that high sequence, such as `1–King–Queen`. |
| Each 1 in a `msaker` of 1s | 10 | If a `msaker` contains three or four 1s, each 1 counts as 10. |
| 1 left uncombined in a losing player's hand | 1 | For the end-of-hand remaining-card penalty, an uncombined 1 always counts as 1 point. |
| Number cards 2–10 | The number shown (2–10) | Each card is worth its face value. |
| Jack | 10 | Picture card. |
| Queen | 10 | Picture card. |
| King | 10 | Picture card. |
| Joker in a placed combination | Value of the card it replaces | Its combination/drop value follows the represented card. |
| Joker left uncombined in a losing player's hand (Solo) | 10 | Fixed end-of-hand penalty in Solo mode. |
| Joker left uncombined in a losing player's hand (2v2) | 20 | Fixed end-of-hand penalty in 2v2 mode. |

### 12.1 Combination-value examples

The following examples establish how the combination total is calculated:

| Combination | Calculation | Total |
|---|---:|---:|
| `1♥ + K♥ + Q♥` | 10 + 10 + 10 | **30 points** |
| `1♥ + 1♠ + 1♦ + 1♣` | 10 + 10 + 10 + 10 | **40 points** |
| `1♥ + 1♠ + 1♦` | 10 + 10 + 10 | **30 points** |
| `1♥ + 2♥ + 3♥ + 4♥` | 1 + 2 + 3 + 4 | **10 points** |

The same calculation applies to other combinations: add the point value of every card, using the context-specific value of 1 and the represented-card value of any Joker.

### 12.2 Values of uncombined cards left in a losing player's hand

These are end-of-hand values used to calculate the reduced penalty for a losing player who made a qualifying score-reduction drop. They apply to cards remaining in that player's hand that have not been included in a placed combination. A losing player who did not make a qualifying score-reduction drop receives a fixed 100-point penalty instead, regardless of their remaining cards.

| Remaining card | Solo penalty | 2v2 penalty |
|---|---:|---:|
| 1 | 1 point | 1 point |
| 2–10 | Face value | Face value |
| Jack, Queen, King | 10 points each | 10 points each |
| Joker | 10 points | 20 points |

Therefore, the value of a 1 depends on its context: in an uncombined losing hand it counts as **1 point**; in a low sequence it counts as 1; and in the specified high sequence or a `msaker` of 1s it counts as 10. A Joker's value also depends on its context: in a combination it takes the value of the card it represents, while an uncombined Joker left in a losing hand counts as 10 in Solo or 20 in 2v2.

## 13. Open questions and details to confirm

The following points are intentionally left open rather than guessed. They should be resolved as the rules are documented further.

### Opening, placing, and winning
No unresolved questions are currently listed in this category. The ordinary place-to-win sequence is documented in Section 9.

## 14. Terminology glossary

| Term | Meaning in this rulebook |
|---|---|
| **ymachki** | The pre-deal step in which the player on the dealer's left randomly divides all 108 face-down cards into exactly two portions, chooses one of those portions, and gives it to the dealer to start dealing. |
| **war9a** | One of the three dealing styles. |
| **azwez** | One of the three dealing styles; starts with 2 cards for the dealer. |
| **3adiya** | Normal dealing style; starts with 3 cards for the dealer. |
| **msaker** | A same-rank combination using at least 3 different suits. |
| **ma7loul** | A consecutive sequence in the same suit. |
| **place** | Adding a card to a combination already placed on the table; it can be performed only during the active player's own turn. |
| **score-reduction drop** | Laying down qualifying combinations whose total value reaches the current minimum, reducing the penalty if another player wins. |
| **tour** | One complete pass of the dealing sequence. In four-player `war9a`, after the first tour (1 card to the dealer and 2 to each other player), repeated tours give 2 cards to each player. |
| **split/stock** | The face-down draw pile. A player draws only its top card. When it runs out, the player may take the latest discard only if it enables a qualifying score-reduction drop or a full immediate win. Otherwise, the latest discard stays face up while all older discards are collected, shuffled, turned face down, and used as the new stock; the player draws its top card. |

---

## Change log

- **v4.0 — 12 November 2026:** Clarified how a player wins by `place` when they have only one unmatched card left: on their turn, they first draw one card, place the previously held unmatched card onto a valid table combination, and discard the card just drawn. Placing the final unmatched card before drawing does not end the hand by itself. Updated Section 9 and removed the resolved ordinary place-to-win timing question.
- **v3.9 — 12 November 2026:** Confirmed that the player performing `ymachki` (the player on the dealer's left who divides the 108 face-down cards into exactly two portions) chooses which portion to give to the dealer. Updated Section 3.2 and the glossary; removed the resolved portion-selection question.
- **v3.8 — 12 November 2026:** Confirmed that `ymachki` divides all 108 face-down cards into exactly two portions. The player on the dealer's left gives one of those portions to the dealer to start dealing. The convention for choosing which portion is handed to the dealer remains to be clarified. Updated Section 3.2 and the glossary; narrowed the open question to portion selection.
- **v3.7 — 12 November 2026:** Clarified the two choices when the stock is empty: take the latest discard only to make a qualifying score-reduction drop or win immediately; otherwise, leave it face up, shuffle all older discards into a new face-down stock, and draw its top card. Added the four-player example (52 central discards at stock exhaustion: latest discard stays, 51 older cards are recycled) and the corresponding two- and three-player counts (80 and 66). Removed the one-card discard-pile edge-case question because the normal card-count flow leaves multiple older discards available.
- **v3.6 — 12 November 2026:** Confirmed that when the stock is empty and the player cannot take the latest discard, they leave that latest card face up (for example, skip `7♥`) and draw from the replenished stock made from older discards. Clarified that the latest discard is never included in the reshuffle. Added an open question for the rare edge case where the latest discard is literally the only card in the central pile and therefore no older discards are available to form the replacement stock.
- **v3.5 — 12 November 2026:** Clarified stock recycling when the face-down stock runs out: the most recently discarded card stays face up in the centre as the current discard, while all other/older cards in the central discard pile are collected, shuffled, turned face down, and used to form the new stock. The player draws the top card from that new stock. The latest discard is excluded from the reshuffle and remains subject to the normal special pickup conditions.
- **v3.4 — 12 November 2026:** Confirmed that an eligible pickup of the immediately preceding player's latest discard takes priority when the face-down stock/split is empty. The player may still take that discard to win immediately or make a qualifying score-reduction drop; recycling is performed only when the player is not eligible to take the discard and needs to draw from the stock. Removed the resolved stock-recycling timing question.
- **v3.3 — 12 November 2026:** Confirmed that when the face-down stock/split runs out, the cards in the central face-up discard pile are collected, shuffled, turned face down, and used as the replenished stock/split. The timing relative to eligible discard pickup was not yet confirmed.
- **v3.2 — 12 November 2026:** Confirmed that one designated player keeps the score on paper throughout the game; the scorekeeper role does not rotate between players. Confirmed that the first distribution is performed by this scorekeeper. Removed the resolved scorekeeper question.
- **v3.1 — 12 November 2026:** Confirmed the match-loss condition for the selected 401/601/801 target: the first Solo player or 2v2 team whose cumulative score reaches or passes the target loses. In Solo, if two or three players reach/pass the target in the same hand's score update, all of those players lose. In 2v2, the target is checked against the combined cumulative score of the two teammates. Removed the resolved match-target question.
- **v3.0 — 12 November 2026:** Confirmed that the winner always receives −10 points in Solo and the winning team always receives −20 points in 2v2, regardless of the method used to win (normal completion, taking the immediately preceding player's discard, `place`, or eligible Joker reclamation). Clarified that the 2v2 −20 is recorded as the winning team's combined score for the hand. Removed the resolved Solo and 2v2 winner-score questions.
- **v2.9 — 12 November 2026:** Confirmed that a losing player who did not make a qualifying score-reduction drop receives a fixed 100-point penalty, regardless of the cards remaining in their hand. A player who did make a qualifying drop receives the value of their remaining uncombined cards. Clarified this rule for Solo and 2v2 scoring, updated the Solo example interpretation and Section 12.2, and removed the resolved default-100 question.
- **v2.8 — 12 November 2026:** Confirmed that the escalating score-reduction minimum is shared by all players. After any qualifying drop, the global threshold becomes that drop's total value plus 1; every player must meet the same updated threshold for their next qualifying drop. Removed the resolved scope question.
- **v2.7 — 12 November 2026:** Clarified the escalating score-reduction minimum with a chained example: starting minimum 71, a qualifying drop worth 80 sets the next minimum to 81; if the next qualifying drop is worth 81, the following minimum becomes 82. Confirmed the formula as the most recent qualifying drop total plus 1. Whether the threshold is shared by all players or tracked per player remains open.
- **v2.6 — 12 November 2026:** Confirmed that `place` may be performed on any valid combination already on the table, regardless of who laid it down. In 2v2, this includes a teammate's combinations as well as opponents' combinations. Updated Section 9 and removed the resolved place-target question from the open questions.
- **v2.5 — 12 November 2026:** Confirmed that players may perform actions only during their own turns. Added a general turn-ownership rule covering drawing/picking up cards, laying down combinations, score-reduction drops, `place`, Joker reclamation, and the final discard. Updated the `place` section and removed its timing question from the open questions.
- **v2.4 — 12 November 2026:** Confirmed that before the game starts, the players choose the initial score-reduction minimum from 51, 61, or 71 points. Updated Section 8.1 and removed the resolved starting-minimum question from the open questions.
- **v2.3 — 12 November 2026:** Clarified that a losing player cannot have a 0-point remaining-card penalty: having no cards left after completing the required final discard would mean that player had won and ended the hand. Therefore, a 2v2 losing-team example such as `0 + 100` is impossible.
- **v2.2 — 12 November 2026:** Confirmed that in 2v2 mode, the losing team's penalty is the sum of both teammates' individual penalties. A teammate who made a qualifying score-reduction drop contributes the value of their remaining uncombined cards; a teammate who did not drop contributes the standard 100 points. Added the example `14 + 100 = 114`, and clarified that two players who did not drop contribute `100 + 100 = 200` to the losing team's total.
- **v2.1 — 12 November 2026:** Confirmed the end-of-hand penalty values for uncombined cards remaining in a losing player's hand: 1 counts as 1 point; number cards 2–10 count at face value; Jack, Queen, and King count as 10 each; an uncombined Joker counts as 10 in Solo and 20 in 2v2. Distinguished an uncombined Joker's penalty value from a Joker used in a combination, where it still takes the value of the card it represents. Removed the resolved questions about an uncombined 1 and an uncombined Joker.
- **v2.0 — 12 November 2026:** Clarified that “win immediately” during Joker reclamation means the player can complete the entire winning hand arrangement, not merely place the missing card(s) into the opponent's combination; the player then discards their final unmatched card. Added the player's `msaker` and `ma7loul` examples. Reworked card valuation with explicit context-dependent 1 values, number/face-card values, Joker valuation, and combination-total examples (`1♥ + K♥ + Q♥ = 30`, four 1s = 40, three 1s = 30, and `1♥ + 2♥ + 3♥ + 4♥ = 10`).
- **v1.9 — 12 November 2026:** Confirmed how to reclaim a Joker from a placed `ma7loul`: add the exact missing real card represented by the Joker (for example, add `6♥` to `5♥ + Joker + 7♥`) and take the Joker. The same eligibility conditions as `msaker` retrieval apply: the player must qualify for a score-reduction drop, or, if they cannot meet the minimum, the action must allow them to win immediately. Also reconciled the open-question list with the already-confirmed six-card sequence rule: split it into two three-card combinations.
- **v1.6 — 12 November 2026:** Confirmed that a Joker can substitute for a missing card in both `msaker` and `ma7loul`. Added examples for a Joker completing a same-rank, different-suit `msaker` and a same-suit consecutive `ma7loul`; removed the question about which combination types allow Joker substitution.
- **v1.5 — 12 November 2026:** Confirmed that the same dealing logic and hand sizes apply with 2, 3, or 4 players: the dealer receives 15 cards and each other player receives 14. Updated all three dealing styles to apply to any supported player count, and corrected the undealt stock sizes (79 cards with 2 players, 65 with 3, and 51 with 4).
- **v1.4 — 12 November 2026:** Confirmed the four-player `azwez` sequence: the first tour gives 2 cards to the dealer and 2 to each other player; each of the following six tours gives 2 cards to every player; after tour 7, everyone has 14 cards, then the dealer takes 1 additional card to reach 15 (57 cards dealt total).
- **v1.3 — 12 November 2026:** Confirmed the four-player `3adiya` sequence: the first tour gives 3 cards to the dealer and 2 to each other player; each of the following six tours gives 2 cards to every player; after tour 7, the dealer has 15 cards and each other player has 14 (57 cards dealt total).
- **v1.2 — 12 November 2026:** Documented the exact four-player `war9a` sequence: one card to the dealer and two to each other player in the first tour; then two cards to each player in repeated tours until the other players have 14 and the dealer has 13; finally, two more cards to the dealer (15 total), who discards one after inspecting their hand. The adaptation for 2- and 3-player games remains open.
- **v1.1 — 12 November 2026:** Clarified `ymachki`: the player on the dealer's left randomly splits the face-down cards and gives a portion to the dealer to start dealing. The number of portions and exact selection convention remain open.
- **v1.0 — 12 November 2026:** Confirmed that a score-reduction drop must meet the full current minimum in one turn. Players cannot accumulate partial combination drops across multiple turns.
- **v0.9 — 12 November 2026:** Clarified `msaker` size with explicit examples: the minimum is 3 cards of the same rank in 3 different suits, and it may contain all 4 suits. Red and blue card backs may be mixed freely.
- **v0.8 — 12 November 2026:** Confirmed that card-back colour does not restrict combinations. A combination may mix red-backed and blue-backed cards; `msaker` still requires different suits.
- **v0.7 — 12 November 2026:** Confirmed that the remaining stock/split is kept face down with card faces hidden, and players may draw only the top card.
- **v0.1 — 12 November 2026:** Created the initial rules draft from the player's explanation. Preserved Tunisian terminology and marked unresolved mechanics as open questions.
- **v0.2 — 12 November 2026:** Clarified when a player may take the previous player's discard; added card point values and Ace/1 exceptions; documented Joker valuation and the escalating minimum for score-reduction drops.
- **v0.3 — 12 November 2026:** Clarified the normal turn count: a player starts with 14 cards, draws one card to reach 15, and must discard one card before ending the turn. This discard requirement also applies after a qualifying score-reduction drop; a winning play ends the hand instead.
- **v0.4 — 12 November 2026:** Confirmed that all players place discards in one shared pile at the centre of the table; removed the separate-piles question.
- **v0.5 — 12 November 2026:** Clarified that a player may take only the card just discarded by the immediately preceding player (on their left), and only when it enables an immediate win or a qualifying score-reduction drop. Otherwise, the player must draw from the stock/split; older discards in the central pile are never eligible.
- **v0.6 — 12 November 2026:** Confirmed that the shared discard pile in the centre of the table is kept face up, allowing all players to see the discards. The pickup restriction remains unchanged: only the immediately preceding player's latest discard may be taken, and only when a special pickup condition is met.
