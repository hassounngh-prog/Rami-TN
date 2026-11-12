# Scope

**Rigor**: standard — features 3b, 6, 7 override to `full`.

Private online Tunisian Rami for exactly four human players, individual (Solo) and 2v2 modes, Android first. Audience already knows the game: no tutorial, no AI opponents, no public matchmaking. Build order is local-playable-first, then online. Rules follow `Tunisian_Rami_Rules_v4.0.md` with `docs/glossary.md` clarifications where they explicitly supersede it.

| # | Feature | Blocked by | Status |
|---|---|---|---|
| 1 | [Deck, cut & deal](specs/0001-deck-cut-deal.md) | none | done |
| 2 | [Combinations & card values](specs/0002-combinations-card-values.md) | 1 | done |
| 3a | [Turn flow & drop — Solo hand](specs/0003-turn-flow-drop.md) | 1, 2 | done |
| 3b | Place, Joker reclamation & win — Solo hand `full` | 3a | planned · needs spec |
| 4 | Scoring & match — Solo | 3a, 3b | planned |
| 5 | Local playable table | 3b, 4 | planned · needs spec |
| 6 | Private online table — Solo `full` | 5 | planned · needs spec |
| 7 | Disconnect pause & rejoin `full` | 6 | planned · needs spec |
| 8 | 2v2 mode | 3b, 4, 6 | planned |

## 1. Deck, cut & deal

Intent: prepare a hand automatically, exactly as the physical game does.

Done when: a four-player deal yields dealer 15 (→14 after the opening discard), everyone else 14, and the correct stock size in all three dealing styles, verified by tests.

- [x] 108-card deck (two back colours + four Jokers); back colour never restricts combinations
- [x] ymachki: cutter splits the face-down deck into two portions and chooses which the dealer takes
- [x] Three dealing styles (war9a, azwez, 3adiya), right-to-left, correct hand/stock counts
- [x] Dealer opening discard to start play

## 2. Combinations & card values

Intent: decide which card groups are legal and what they are worth.

Done when: the validator accepts/rejects msaker and ma7loul (including Jokers and the six-card split) and computes combination and remaining-card values per rulebook + glossary, verified by tests.

- [x] msaker: 3–4 same rank, distinct suits
- [x] ma7loul: 3–5 consecutive same suit; 1 low/high; Ace-high 1–K–Q / 1–K–Q–J / 1–K–Q–J–10 only (1 at bottom); no K–1–2 wrap; 6+ run split into valid 3–5-card groups, player-declared (six → two 3s)
- [x] Joker substitution + limits (msaker ≤3, ma7loul ≤4, ≥1 real card); declared bottom-to-top order fixes represented ranks/values
- [x] Context-dependent values (1 low = 1, 1 high/msaker = 10, faces = 10, Joker = represented; uncombined 1 = 1, uncombined Joker 10 Solo / 20 2v2)

§5.2 ma7loul detail (Ace-high forms, long-run splitting) resolved by owner decision — see glossary `ma7loul` and `Long-run split`. Contract and confirmed seams: spec 0002.

## 3a. Turn flow & drop — Solo hand

Intent: the hand advances turn by turn, and a player can qualify by dropping combinations in one turn.

Done when: tests play a Solo hand from the dealer's opening discard through legal turns — draw or eligible pickup, qualifying drop, required discard — reject every illegal action, and recycle the stock at exhaustion; the hand still ends only through feature 3b's win.

- [x] Own-turn-only actions; 14 → draw → 15 → discard → 14 turn count
- [x] Turn order follows the dealing direction: after the dealer's opening discard the dealer's right-hand neighbour acts first, so the latest discard is always the card of the player on the current player's left — confirmed against feature 1's seat arithmetic (spec 0003)
- [x] Draw only the top stock card
- [x] Discard pickup for a qualifying drop: the immediately preceding player's latest discard only, retained while that same turn's combinations meet the current minimum
- [x] Score-reduction drop: single-turn, shared escalating minimum (51/61/71 start; next = last qualifying drop total + 1)
- [x] Minimum resets to the chosen start value at every new hand (owner decision, this round)
- [x] First-place eligibility is recorded: the session flags who has dropped, so feature 3b can refuse ordinary place to a player who has not qualified — the refusal itself is 3b scope
- [x] Stock exhaustion: latest discard stays face up, all older discards shuffled into the new face-down stock, player draws its top card
- [x] Hand state carried forward: whose turn, who has dropped, current minimum, stock, shared discard pile

Pure C# rules module, verified by tests in the existing harness, as with features 1–2. No Unity editor is installed and this feature has no UI or server, so the app-level pass (playing a hand on screen) happens in feature 5.

## 3b. Place, Joker reclamation & win — Solo hand `full`

Intent: the table is shared — players build on one another's combinations, take back Jokers, and end the hand.

Done when: tests place onto any player's combination (including an extension that forces the six-card split), reclaim Jokers under the current minimum in the msaker, ma7loul and multi-Joker cases, and end a hand by every winning route — normal completion, discard pickup, place, reclamation — with each below-the-minimum and out-of-turn variant rejected.

- [ ] Place during own turn onto any player's combination: run extension, fourth card of a msaker
- [ ] A place that makes a table run six cards triggers the forced split; the declared bottom-to-top order stays fixed
- [ ] Joker reclamation gated on the current minimum: msaker (both missing real suits), ma7loul (exact represented card), individual replacement from a multi-Joker sequence
- [ ] Winning place without Joker retrieval while below the minimum: final unmatched card into a Joker-bearing combination, then discard
- [ ] Immediate-win discard pickup, allowed only when the whole hand finishes
- [ ] Win = empty hand after the required final discard, whichever route got there
- [ ] Rejections: below-minimum drop, place or reclamation by a player who has not qualified, out of turn, placing the final card before drawing

Capability limit (owner decision, this round): the engine validates arrangements the player declares and answers legality and value. It does not search a hand for a possible win or list every legal move. So feature 3 needs no combinatorial solver, and feature 5's UI shows what the player selected rather than what exists.

Needs spec: pin the ordering of actions inside one turn — whether ordinary place or reclamation may precede the qualifying drop in that same turn, and whether a reclaimed Joker may be used in that same turn's drop. The glossary fixes reclamation's minimum gate but not intra-turn order; open rule, owner decision.

Joker-retrieval timing is no longer open: the glossary resolves it to "must meet the current minimum" and supersedes the rulebook's win exception (§6.1, §6.2); the only below-minimum finish is winning place without retrieval.

## 4. Scoring & match — Solo

Intent: turn a finished hand into scores and run a multi-hand match to a target.

Done when: a Solo match accumulates per-hand scores across hands and correctly declares the loser(s) at the target, verified by tests.

- [ ] Hand scoring: winner −10; no-drop loser +100; dropped loser = value of remaining uncombined cards
- [ ] Cumulative score; match target 401/601/801; first to reach/pass loses; simultaneous multi-player loss
- [ ] Dealer/cutter rotation between hands; fixed scorekeeper
- [ ] Per-hand setup carried forward: target, starting minimum (reset each hand, per 3a), dealing style

## 5. Local playable table

Intent: let a person play a complete Solo match on one device (pass-and-play) through the 3D table.

Done when: on one device, players set up a match, watch automatic cut/deal, take legal actions by touch, and play multiple hands to a target with visible scores.

- [ ] Stylized low-poly 3D table, fixed elevated camera; hands, shared discard, stock, table combinations, scoreboard
- [ ] Automatic cut & deal animation that preserves the rules
- [ ] Touch input for draw/pickup, drop, place, reclaim, discard; turn indicator; UI offers only what the turn state allows, illegal actions rejected at the rules boundary
- [ ] Local match setup (target, starting minimum, dealing style), multi-hand flow, results

Needs spec: presentation and interaction design (camera, layout, card selection, action UX). Pass-and-play is a build milestone to validate rules and feel; the shipped product is online-only (feature 6).

## 6. Private online table — Solo `full`

Intent: four friends on separate phones play a private Solo match together.

Done when: four devices join one table by room code + display name and play a complete networked Solo match with consistent state and results.

- [ ] Host creates a table; join by room code + display name; four seats; no accounts
- [ ] Pre-game setup shared with local (target, minimum, dealing style); seat readiness
- [ ] Networked game state: turn ownership, action validation, and consistent state across devices
- [ ] Multi-hand progression, rotation, and match-end results across devices

Needs spec: networking/backend design and state-authority model (undecided per AGENTS.md).

## 7. Disconnect pause & rejoin `full`

Intent: a dropped player can come back without losing the match.

Done when: a player disconnects mid-match, the table pauses, and they rejoin their seat with state intact and play resumes.

- [ ] Detect disconnect; pause the table; hold the seat
- [ ] Persist match state across the gap
- [ ] Rejoin by code + name into the same seat; resume consistent state

Needs spec: state persistence and rejoin approach.

## 8. 2v2 mode

Intent: four players play as two teams with combined scoring.

Done when: four players play a networked 2v2 match with correct team scoring, combined target check, and results.

- [ ] Team assignment + seating; place allowed on teammate and opponent combinations
- [ ] Team scoring: winner team −20; losing team = sum of both teammates' penalties (dropped = remaining value, no-drop = 100)
- [ ] Combined cumulative team score vs target; first team to reach/pass loses
- [ ] 2v2 setup in lobby (local + online)

## Out of scope (first release)

- Two- and three-player games (exactly four players only)
- AI/computer opponents, offline or single-player play
- Public matchmaking
- Player accounts / login (room code + display name instead)
- Tutorial-first onboarding
- iOS at launch (Android first; iOS later)
- Automatic arrangement search, win hints, or listing every legal move — the engine validates the combinations the player declares

## Open decisions

- Intra-turn action ordering (may ordinary place or Joker reclamation precede that turn's qualifying drop; may a reclaimed Joker be used in it) → owner decision before feature 3b is done.
- Presentation/interaction (5), networking + state authority (6), persistence/rejoin (7) → `/spec`.
- Real-device perf profiling (30 FPS low-end / 60 FPS mid-high) → verify during/after feature 5; these are targets, not measured results.