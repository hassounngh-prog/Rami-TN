# 0003: Turn flow & drop — Solo hand

**Status**: Accepted
**Feature**: 3a — Turn flow & drop (Solo)

## Problem

Four friends playing at one table expect the app to run a hand exactly the way they do: you act only on your own turn, you take exactly one extra card, you may lay down combinations only once they clear the current minimum, and you must throw one card away to pass the turn. When the face-down stock runs out, the older discards are shuffled into a new stock while the newest discard stays in the middle. These players know the rules by heart, so the app must never allow what a table would reject, and never block what a table would allow.

## Acceptance criteria

- AC-1: **Hand start.** Given a feature 1 deal whose dealer has made the opening discard, the session exposes 14 cards for each of the four seats, a 51-card stock, the opening discard as the latest discard, and `(dealer + 1) % 4` as the active seat.
- AC-2: **Draw.** The active player takes exactly one card, only the top card of the stock, and holds 15. Drawing a second time in the same turn, drawing when the hand already holds 15, or drawing for a seat that is not active is rejected. Deeper stock cards are never selectable.
- AC-3: **Qualifying drop.** The active player lays down a declared set of combinations, each one validated by feature 2, and the drop is accepted only when the combined value reaches at least the current minimum. On acceptance those cards leave the hand, the seat is marked as having dropped, and the current minimum becomes that drop's total + 1 for every player. Rejected: combined value below the minimum, an illegal declared group, a drop before the turn's extra card is taken, and a second drop in the same turn.
- AC-4: **Single-turn requirement.** A drop counts only the combinations laid down in that turn. A player cannot combine values from earlier turns to reach the minimum; a player who already dropped must still clear the current minimum with each new set of combinations.
- AC-5: **Discard pickup.** A drop may declare its extra card as the immediately preceding player's latest discard instead of drawing from the stock. That is accepted only when the same turn's drop clears the current minimum; the picked-up card may stay in hand. Rejected: any older discard, another seat's discard, pickup with a below-minimum drop, pickup after drawing this turn, and pickup instead of a required discard.
- AC-6: **Required discard.** A turn closes only with exactly one discard from the cards still held. After it, the seat holds 14 minus the cards that turn dropped, the discarded card is face up as the new latest discard, the active seat becomes `(seat + 1) % 4`, and the current minimum is unchanged. Ending a turn without a discard, or discarding twice, is rejected.
- AC-7: **Turn ownership.** Every action by a seat other than the active seat is rejected. The active seat advances in the +1 direction all hand, so the immediately preceding actor is always `(seat + 3) % 4` — the actor's left neighbour, the seat the glossary calls the eligible pickup source.
- AC-8: **Stock exhaustion.** When the active player still owes an extra card and the stock is empty, they either take the latest discard under AC-5 or leave it face up in the centre while all older discards are shuffled with the session seed, turned face down, and become the new stock; they then draw its top card. The latest discard is never part of the recycled stock and stays eligible for the next player. Four-player example: after 51 stock cards are drawn the centre holds 52 (latest + 51 older), and the new stock holds 51.
- AC-9: **Rejected actions change nothing.** After any rejection, hand sizes, active seat, stock contents and order, discard pile contents and order, current minimum, and dropped flags are identical to before the attempt.
- AC-10: **Per-hand scope and reset.** A hand is one session. A new session starts with the current minimum reset to the match setup value (51, 61 or 71) and no seat marked as dropped. Escalation never crosses a hand boundary.
- AC-11: **Table record.** Each accepted drop records, per owning seat, the combination kind, its cards in the declared bottom-to-top order, each Joker's represented card, and its value — readable from the session, so feature 3b can place onto these combinations. This spec offers no place, reclamation, or win action.

## Delivery order

1. Session over a completed deal, turn order, draw, required discard, hand-count invariants (AC-1, AC-2, AC-6, AC-7, AC-9).
2. Drop with feature 2 validation, single-turn minimum, escalation, dropped flags, table record (AC-3, AC-4, AC-10, AC-11).
3. Discard pickup folded into the drop (AC-5).
4. Stock exhaustion and seeded recycling (AC-8).

## Decisions

- Mutable hand-session object, constructed from a feature 1 `DealResult` after the opening discard plus per-hand setup (starting minimum, seed). One action per call — draw, drop, discard — with a read-only view of the state. Matches how `DealResult.OpeningDiscard` already behaves; no state-machine framework, no command objects.
- Pickup is a parameter of the drop, not a standalone action. A turn's one extra card is therefore either the stock top or the declared pickup, and an illegal pickup can never be committed, so nothing needs unwinding.
- Pickup eligibility checks only that the same turn's declared drop clears the current minimum. It does not prove the picked-up card was necessary, because the glossary lets the player retain it, and proving necessity would require searching the hand — ruled out by feature 3b's capability limit. This matches a table where the player announces the drop and the other players accept it.
- One drop per turn: all qualifying combinations are declared in a single drop. The rulebook has no support for several separate drops in one turn, and after a qualifying drop the minimum has already risen above that total.
- Every new set of combinations must clear the current minimum, for every player every time; the minimum is shared, escalates to the last qualifying drop total + 1, and resets to the setup value each new hand (glossary `Shared minimum`, owner decision recorded in scope 3a).
- Illegal gameplay actions return a rejection with a reason instead of throwing, so feature 5's UI can explain the refusal and feature 6 can report it to a client. Throwing stays reserved for setup errors, as in feature 1.
- Recycling shuffles with `System.Random(seed)` using the same Fisher-Yates loop as `Dealing.Deal`, so tests are deterministic and the module keeps one randomness convention.
- Solo only: no mode parameter. 2v2 place rules, team checks, and Joker penalties stay in feature 8.
- Turn order reuses feature 1's seat arithmetic rather than inventing a direction: active seat `(dealer + 1) % 4`, advancing +1. Existing `DealResult.NextToAct` and `Cutter = (Dealer + 3) % 4` already encode it, which closes the inferred open decision recorded in scope.

## Value sources

| Value the build must produce | Where it comes from |
|---|---|
| Four 14-card hands, 51-card stock, latest discard, dealer seat | Feature 1 `DealResult` after `OpeningDiscard` |
| First active seat, seat ordering direction | Derived `(dealer + 1) % 4`; rulebook §3.3–§3.4 + existing `NextToAct` |
| Starting minimum (51 / 61 / 71) | Match setup input, per rulebook §8.1 |
| Current minimum | State: setup value, then last qualifying drop total + 1 (§8.3) |
| Combination legality, declared order, Joker representations, value | Feature 2 `Combinations.Validate` → `Combination` |
| Drawn card | Stock top |
| Pickup card | Immediately preceding player's latest discard (shared pile top) |
| Recycled stock order | Fisher-Yates shuffle of all older discards, session seed (§4.1) |
| Dropped flag per seat, table combinations | State, written by each accepted drop |
| Stock/centre counts in the exhaustion example | Rulebook §4.1 four-player example |
| Dealing style | Feature 1 input; unused by turn flow |

## Test seams

| Seam | Catches | Misses |
|---|---|---|
| Hand session: draw / drop (with pickup source) / discard + state reads | Turn order and ownership, 14 → 15 → 14 counts, draw-only-top, single-turn minimum, escalation and per-hand reset, pickup legality, stock exhaustion and recycling, atomic rejection, table record | Place, Joker reclamation, split-on-place, win and the final discard (3b); hand and match scoring, rotation, multi-hand flow (4); UI (5); network and authority (6); 2v2 (8) |
| Existing `Combinations.Validate` / `SplitRun`, `Scoring.RemainingPenalty` | The legality and values a drop consumes, already covered by spec 0002 | Anything turn-level; 3a adds no new tests at this seam |
| Existing `Dealing.Deal` | Deterministic setup feeding hand-session tests | Turn flow itself |

Tests observe the shuffle only through resulting draw order and counts, never through shuffle internals.

## Out of scope

- Place onto any player's combination, the six-card split a place triggers, Joker reclamation, winning by any route (feature 3b).
- Winner −10, no-drop +100, remaining-card penalty totals, match target 401/601/801, dealer and cutter rotation, multi-hand progression (feature 4).
- Presentation, camera, touch input, scorekeeper UI (feature 5).
- Networking, state authority, disconnect and rejoin (features 6, 7).
- 2v2 mode differences (feature 8).
- Searching a hand for a possible win, hints, or listing every legal action (capability limit recorded in scope 3b).

## Open questions

- The centre pile holding fewer than two cards when the stock runs out is impossible in a four-player hand — §4.1's counts give 52. The build should treat it as an invariant rather than code a special case; confirm no rule for two- or three-player play is expected in this release (it is not, per scope).
- Exact class and method names, the rejection reason vocabulary, and the state view's shape are left to `/build`.
