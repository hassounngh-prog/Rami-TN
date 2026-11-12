# 0002: Combinations & card values

**Status**: Accepted
**Feature**: 2 — Combinations & card values

## Problem

Before a turn can be played or scored, the game must know which card groups are legal combinations and what they are worth — including Jokers and the context-dependent value of 1 — so drops, places, and end-of-hand penalties follow the agreed rules exactly. Feature 1 prepares the hand; feature 2 decides legality and value; feature 3 consumes both for turn flow.

## Acceptance criteria

- AC-1: **msaker legality.** Validate accepts 3–4 cards of the same rank with distinct suits (back colour ignored); rejects fewer than 3, more than 4, a repeated suit, or mixed ranks.
- AC-2: **ma7loul legality (low).** Validate accepts 3–5 consecutive ranks of one suit in ascending bottom-to-top order (1–2–3 … 6–7–8–9–10); rejects non-consecutive ranks, mixed suits, fewer than 3, more than 5, and any K–1–2 wraparound.
- AC-3: **ma7loul legality (Ace-high).** Validate accepts exactly 1–K–Q, 1–K–Q–J, and 1–K–Q–J–10 (bottom-to-top, 1 at the bottom, one suit) with high orientation; rejects any other Ace-high arrangement, an orientation that disagrees with the cards, and a 6-card high run holding the 9 (must split). 1 is never both high and low in one sequence.
- AC-4: **Joker substitution + limits.** A Joker represents the specific missing card. Validate accepts msaker with ≤3 Jokers and ma7loul with ≤4 Jokers, each with ≥1 real card; rejects Joker-only groups and over-limit counts. The Combination reports each Joker's represented card: ma7loul = exact rank+suit; msaker = the rank plus the missing suit(s) filled (a lone Joker in a 3-card msaker is not pinned to one suit).
- AC-5: **Declared order + orientation fix represented ranks/value.** For an ordered ma7loul, the real cards, bottom-to-top order, and low/high orientation determine each Joker's represented rank: 6♥ below four Jokers → 6–7–8–9–10 (40); four Jokers below 6♥ → 2–3–4–5–6 (20); 1♥ + two Jokers with low orientation → 1–2–3 (6), with high orientation → 1–K–Q (30). An invalid order, or an orientation that disagrees with the real cards, is rejected. msaker order/orientation is irrelevant.
- AC-6: **Combination value.** The Combination reports its total value using context-dependent card values: 1 in a low ma7loul = 1; 1 in an Ace-high ma7loul or a msaker of 1s = 10; 2–10 = face; J/Q/K = 10; Joker = represented card's value. Verified: 1♥K♥Q♥ = 30; four 1s = 40; three 1s = 30; 1♥2♥3♥4♥ = 10.
- AC-7: **Remaining-card penalty.** RemainingPenalty(cards, mode) sums uncombined card values: 1 = 1; 2–10 = face; J/Q/K = 10; Joker = 10 (Solo) / 20 (2v2). Verified: K,2,J Solo = 22; lone 1 = 1; Joker Solo = 10, 2v2 = 20. The fixed 100 no-drop penalty and winner −10 are not computed here.
- AC-8: **Run splitting.** SplitRun(run) for a same-suit run of 6+ returns the legal partitions into 3–5-card ma7loul groups: six cards → exactly the forced two 3s (4–5–6–7–8–9 → 4–5–6 + 7–8–9); seven → 3+4 and 4+3. Validate rejects any >5 run as a single combination. Every group in every returned partition passes Validate.

## Delivery order

1. Real-card msaker + ma7loul (low and Ace-high) validation and value; remaining-card penalty for real cards (AC-1, AC-2, AC-3, AC-6 real cases, AC-7 real cases).
2. Joker substitution, limits, declared order + orientation → represented ranks/values; Joker remaining penalty by mode (AC-4, AC-5, AC-6 Joker cases, AC-7 Joker cases).
3. SplitRun partitions + >5 rejection as one group (AC-8).

## Decisions

- Pure C# rules module, no Unity types; NUnit EditMode tests at the confirmed seams (matches feature 1). Static classes + sealed result types, like `Dealing`/`DealResult`.
- Three public seams: `Combinations.Validate(cards, orientation)` → Combination or rejection; `Combinations.SplitRun(run)` → partitions; `Scoring.RemainingPenalty(cards, mode)` → int.
- Validate is a query: it returns a Combination on success and rejects illegal input without throwing, so feature 3 can test legality before acting. `/build` picks the exact result shape.
- Declared arrangement = ordered card list (bottom-to-top) + a low/high orientation for ma7loul. Orientation disambiguates 1-at-the-bottom + Jokers (low 1–2–3 vs high 1–K–Q) and must agree with any real cards that already fix direction. msaker ignores order/orientation. (Owner confirmed ordered list + orientation.)
- Bottom-to-top convention: low ma7loul ascends (lowest at bottom); Ace-high ma7loul is 1–K–Q–J–10 with 1 at the bottom. (Owner correction.)
- Combination is a rich result: kind (Msaker/Ma7loul), ordered cards, each Joker's represented card, total value — capable for feature 3 place/reclaim and feature 4 scoring, not a bare bool.
- A msaker Joker represents the rank + the missing suit(s); when several suits are missing the specific suit is not fixed (feature 3 reclamation requires all missing real suits). A ma7loul Joker represents an exact rank+suit.
- Mode (Solo/2v2) is an input to RemainingPenalty only; it selects the uncombined Joker value (10/20). Combination value is mode-independent.
- SplitRun centralizes the 6+ partition rule (six forced to two 3s; 7+ valid partitions) so feature 3's place-triggered split and feature 5's UI reuse it. (Owner confirmed adding SplitRun.)

## Value sources

| Value the build must produce | Where it comes from |
|---|---|
| msaker legality (3–4, distinct suits, back ignored) | Rulebook §5.1 |
| ma7loul low legality (3–5 consecutive, no K–1–2 wrap) | Rulebook §5.2 + glossary `ma7loul` |
| Ace-high forms 1–K–Q / 1–K–Q–J / 1–K–Q–J–10 (1 at bottom) | Glossary `ma7loul` + `Bottom-to-top sequence order` (owner decision/correction, feature 2) |
| Joker substitution + limits (msaker ≤3, ma7loul ≤4, ≥1 real) | Rulebook §6 + glossary `Joker limits` |
| Represented ranks from order + orientation | Glossary `Bottom-to-top sequence order` + `Declared sequence`; caller's ordered cards + low/high input |
| Combination value (1 low=1, 1 high/msaker=10, faces=10, Joker=represented) | Rulebook §12.1 |
| Remaining-card value (1=1, faces=10, Joker 10 Solo / 20 2v2) | Rulebook §12.2 |
| Mode (Solo/2v2) | Caller input |
| 6+ run partitions (six → two 3s; 7+ valid) | Glossary `Six-card split` + `Long-run split` (owner decision) |
| Declared bottom-to-top order + orientation | Caller input |

## Test seams

Confirmed by owner: ordered card list + low/high orientation; SplitRun added.

| Seam | Catches | Misses |
|---|---|---|
| `Combinations.Validate(cards, orientation)` | msaker/ma7loul legality (low + Ace-high), Joker substitution + limits, order + orientation → represented ranks, combination value, >5 rejected as one group | turn flow, place/reclaim, match scoring, UI |
| `Combinations.SplitRun(run)` | 6+ run legal partitions (six forced two 3s; 7+ valid), each group valid | which partition the player picks, place-triggered split timing (feature 3/5) |
| `Scoring.RemainingPenalty(cards, mode)` | uncombined values (1=1, faces=10, Joker 10 Solo / 20 2v2) | no-drop fixed 100, winner −10, team sums (feature 4/8) |

## Out of scope

- Turn flow, draw/pickup, drop, place, Joker reclamation (feature 3) — including triggering the six-card split on a place action.
- Hand/match scoring: winner −10, no-drop fixed 100, cumulative target, dealer/cutter rotation (feature 4).
- 2v2 team scoring sums (feature 8); mode here only selects the uncombined Joker value.
- 3D presentation and UI partition selection (feature 5).
- Networking and state authority (feature 6).
- Choosing which partition the player plays — SplitRun returns legal partitions; the choice is the player's (feature 3/5).

## Open questions

None. Ace-high order corrected (1 at bottom); ordered card list + low/high orientation confirmed; SplitRun confirmed.