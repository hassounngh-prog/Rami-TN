# 0001: Deck, cut & deal

**Status**: Accepted
**Feature**: 1 — Deck, cut & deal

## Problem

Before any turn can be played, a hand must be prepared exactly as the physical game does: a correct 108-card deck, the ymachki cut, and one of the three dealing styles — automatically, without silently changing the rules. Players must end up with the right cards in the right order, every time.

## Acceptance criteria

- AC-1: A created deck contains exactly 108 cards: a red-backed 52 and a blue-backed 52 (each 4 suits × ranks 1–10, J, Q, K), plus 2 red-backed and 2 blue-backed Jokers. No duplicates; every card exposes suit, rank, and back colour (Jokers: back colour only).
- AC-2: Dealing is reproducible and lossless: same dealer, style, and seed produce identical hands, stock, and cut; the 108 cards appear exactly once across all hands plus stock.
- AC-3: ymachki: the cutter is the seat immediately before the dealer in dealing order; the shuffled deck is split into exactly two non-empty portions; the portion handed to the dealer holds at least 57 cards; the deal result reports the cut (both portion sizes, which portion was chosen).
- AC-4: In all three styles with four players: dealer receives 15 cards, every other player 14, stock holds 51; cards are dealt starting with the dealer and continuing right-to-left in seating order.
- AC-5: Each style deals the rulebook's packet sequence, verified against a known deck order: war9a — tour 1 dealer 1 / others 2, tours 2–7 two each, final dealer +2; azwez — seven tours of two each, final dealer +1; 3adiya — tour 1 dealer 3 / others 2, tours 2–7 two each, no final packet.
- AC-6: Opening discard: only the dealer may perform it, only with a card from their 15; the card becomes the first card of the shared face-up discard pile; the dealer is left with 14; the next player to act is the seat after the dealer in dealing order; a second opening discard or an attempt by any other seat is rejected.

## Delivery order

1. Deck factory (AC-1)
2. Seeded shuffle + restricted ymachki cut (AC-2, AC-3)
3. Three dealing styles (AC-4, AC-5)
4. Opening discard (AC-6)

## Decisions

- Exactly four seats; 2- and 3-player dealing is not built (release scope supersedes the rulebook's 2–4 wording).
- Pure C# rules module with no Unity types; tests are EditMode NUnit at the confirmed seams.
- Seating convention: seats are indexed in dealing order; dealing proceeds dealer → next seat index (mod 4), i.e. right-to-left; the cutter is the seat before the dealer (mod 4), i.e. the dealer's left.
- One injected seed drives shuffle and cut, so tests and replays are deterministic.
- Cut restriction (owner decision): the portion handed to the dealer must hold ≥57 cards. Since 2 × 57 > 108, exactly one portion can ever qualify, so the cutter's choice is forced to the larger portion; the split itself is random among splits with both portions non-empty.
- Stock order: the chosen portion's undealt remainder first, then the other portion, face down, order preserved, no reshuffle.
- Back colour is data only; no deck/cut/deal rule reads it (combination rules are feature 2).
- The opening discard is the dealer's chosen card (an input), not an automatic pick; automatic preparation covers shuffle, cut, and deal only.
- No deal-packet log or animation API in this feature; style correctness is verified by final distribution against a fixed deck order. Presentation consumes this later (feature 5).

## Value sources

| Value the build must produce | Where it comes from |
|---|---|
| 108-card composition, back colours, 2+2 Jokers | Rulebook §2 |
| Suits and 13 ranks | Rulebook §2 |
| Cutter seat | Rulebook §3.2 (immediately before dealer) + seating convention |
| Dealing direction and order | Rulebook §3.3 (right-to-left) + seating convention |
| Two non-empty portions; chosen portion ≥57 | Rulebook §3.2 + owner decision (this spec) |
| Shuffle and split randomness | Injected seed (decision) |
| Hand sizes 15/14/14/14; stock 51 | Rulebook §3.3 four-player table |
| Packet sequence per style | Rulebook §3.4 A/B/C |
| Stock card order | Decision: chosen-portion remainder, then other portion |
| Opening discard card | Dealer input at the seam |
| Next player to act after opening discard | Derived: seat after dealer in dealing order |
| Dealer seat for the hand | Caller input (scorekeeper-deals-first belongs to match setup, feature 4) |

## Test seams

Confirmed by owner: three entry points, all pure C# EditMode NUnit.

| Seam | Catches | Misses |
|---|---|---|
| `CreateDeck()` | 108-card composition, backs, Jokers, uniqueness | shuffle, cut, deal |
| `Deal(dealer, style, seed)` | cut structure, dealing order, per-style distribution, hand/stock counts, determinism | animation, UI, networking |
| `OpeningDiscard(card)` | dealer-only 15→14, discard pile start, next-to-act, illegal attempts rejected | later turn flow (feature 3) |

## Out of scope

- Card values and combination validation (feature 2)
- Draw, discard pickup, and turn flow after the opening discard (feature 3)
- Scoring, dealer/cutter rotation between hands, scorekeeper (feature 4)
- Cut/deal animation and 3D presentation (feature 5)
- Networking and state authority (feature 6)
- 2- and 3-player deals (release is exactly four players)

## Open questions

None. The ymachki continuation gap was resolved by owner decision: restrict the cut (chosen portion ≥57).