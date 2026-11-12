# Rami

## Confirmed direction
- Stack: Unity + C# + Universal Render Pipeline (URP); exact editor and package versions are undecided.
- First release: private online tables for exactly four human players, with individual and 2v2 modes; experienced Tunisian Rami players are the primary audience.
- Presentation: stylized low-poly 3D, fixed elevated multiplayer camera, automatic cutting and dealing that preserve the agreed rules.
- Performance targets: 30 FPS on low-end phones and 60 FPS on mid/high-end phones; these are targets, not measured results, and require real-device profiling.
- Architecture: modular and event-driven; isolate C# gameplay rules/state from Unity presentation, animation, rendering, and network transport without adding a general-purpose framework.

## Tooling and commands
- Assumed: Unity Package Manager (UPM) will manage Unity dependencies; do not introduce npm tooling without a concrete requirement.
- Assumed: Unity Test Framework/NUnit, with rule tests under `Assets/Tests/EditMode/` and Unity integration tests under `Assets/Tests/PlayMode/`.
- Assumed: four-space C# indentation, IDE formatting, and compiler/IDE diagnostics; no standalone formatter or linter has been selected or installed.
- Pure C# rules module (`Assets/Scripts/Rules/`, no Unity types) and NUnit rule tests (`Assets/Tests/EditMode/`) exist with asmdefs; no Unity project, editor, package manifest, or lockfile is installed yet.
- Verified: .NET SDK 10.0.404, user-local at `C:\Users\asus\.dotnet`. Test: `~/.dotnet/dotnet test tools/RulesTests`; typecheck/build: `~/.dotnet/dotnet build tools/RulesTests`. The harness links the Assets sources; the same NUnit files run as EditMode tests once a Unity editor is installed. Unity install/dev/play/build commands remain unavailable.
- Target operating systems, benchmark devices, backend/networking design, and build output directories remain undecided.

## Rules and scope
- Read both game documents linked below; use confirmed glossary clarifications where they explicitly supersede the original working draft, not generic rummy assumptions.
- Unresolved rules require the owner's decision before the affected feature is implemented; glossary clarifications supersede the rulebook where explicit (Joker-retrieval timing is resolved there: reclamation must meet the current minimum).
- The four-player release scope above supersedes older two-to-four-player release wording in the glossary; both game modes remain included.
- Keep secrets local and Git-ignored; never commit credentials or environment files.

## Workflow and boundaries
- Work only inside this project; do not read or modify sibling projects or operate on the parent Git repository.
- Do not hand-edit generated Unity caches such as `Library/`, `Temp/`, `obj/`, or `Logs/`, or Git internals.
- The documentation-only setup phase is over; implementation proceeds per `docs/scope.md`. Fill documentation gaps without overwriting existing files.
- Use Conventional Commits (`feat:`, `fix:`, `chore:`); commit or push only when explicitly requested.
- Skills, MCP tools, and plugins are opt-in by name per request; Ponytail is the exception and must be loaded at session start unless disabled by the user.
- Address the user as Hassoun and follow the inherited workspace rules in [../../agents.md](../../agents.md).

## Context files
- [Tunisian_Rami_Rules_v4.0.md](Tunisian_Rami_Rules_v4.0.md): original working rulebook, with some superseded rules and open questions.
- [docs/glossary.md](docs/glossary.md): terminology and confirmed rule clarifications from the design discussion.
- [docs/scope.md](docs/scope.md): feature plan and status. [docs/specs/](docs/specs/): feature specs (0001 deck, cut & deal — Accepted). [docs/adr/](docs/adr/): reserved for architecture decisions, still empty.
