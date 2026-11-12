# Consolidation plan — phases, and what each one costs in drives

This plans the work left after the three steps already shipped (meet cache `28ba334`, log writer `b965ea9`,
sleeper split `21df363`, notifications `28cdc17`/`606759c`). Every step runs the agreed loop: my own recon,
a written proposal to the reviewers before code, then their review plus mine after it.

**The rule that shapes the phases:** a behaviour-touching change cannot be proven by a build. The agent
compiles, the user drives. A phase is sized to *one* drive wherever the change can move gameplay, and a
phase that cannot be proven without a drive is never bundled with one that can.

Reviewed by @glm on 2026-11-12 before implementation; its four adjustments are folded in and marked.

## Phase 0 — publish what exists (no drive)

Nothing new is written; what exists becomes the baseline. **Bundled with 1a**, since neither needs a
drive and both are assertions about the current state rather than changes to it.

- push the local commits (6 at the time of writing)
- land `docs/RECON.md` on `main` — it lives on `refactor/split` and answers "what is redundant"
- correct its stale line about the revival never having been driven (it has: load, spawn, races,
despawn, four-car grids)
- exit: `main` is the single source of truth for code *and* plan, tree clean, deployed DLL matches the build

## Phase 1 — Settings table (bundled 1a, then one drive)

The `config.ini` is the user's tuning surface and the shipped defaults are the contract.

- **1a** a table of `(section, key, default, setter)` entries plus one loop. **No drive.** The table is
  asserted to reproduce today's values from the shipped ini on the host, which is a comparison, not a
  behaviour change.
- **1b** `LoadSettings` becomes that loop; the 38-line block goes. The statics stay — **feed them, do not
  migrate readers** (glm: migrating `Racer.cs`/`Util.cs` in the same phase makes one drive vouch for two
  different bindings).
- **1c** document in `docs/RUNTIME.md` that settings are read at load **and re-read in the spawn path**.
- **drive:** change exactly one visible value in the shipped ini, load, confirm the effect, change it back.

**Verified before planning, not assumed:** all 38 reads are already
`Field = val.GetValue<T>(section, key, literalDefault)` — a missing key *already* resets the field to a
literal default. So a table cannot "clobber in-progress values" on the mid-session re-read: that reset is
today's behaviour and the table reproduces it. Exactly **one** line is not a plain read —
`SpeedMultiplier *= 0.01f` — and it belongs after the loop.

**risk:** the float parse (invariant culture) and the `*0.01f` transform. Both testable on the host.

## Phase 2 — the monoliths (one drive per step)

`OnTickBody` first, deliberately (glm: the spine is the least forgiving place to calibrate on, and doing it
first makes the state machine's dispatch step a formality). `HandleMeet`'s cleaner boundary is not a reason
to start there.

`OnTickBody` is 686 lines with six regions already visible in the code:

| region | today | what it does |
|---|---|---|
| a | 494-502 | ambient chatter (Lamar comments, gated) |
| b | 503-646 | cheat-string input: invite, save car(s), help, shorten track, base bet, bet, leave, despawn |
| c | 647-807 | player registration and joining, including the vehicle-becomes-racer path |
| d | 808-819 | spectator upkeep |
| e | 820 | meet presence: spawn or despawn by player position |
| f | 821+ | timers (grid top-up, crowd reactions) and the state machine call |

- **2a** extract a, b, d, e, f as named stages called in the same order. Region **c** moves last, on its
  own, because it creates racers and moves money. **The order of e before f is load-bearing** — presence
  can jump the machine to `NotStarted` in the same tick — so the extraction must preserve call order, not
  just call presence.
- **2b** `HandleMeet`: one method per state; the switch becomes a dispatch.
- **2c** `HandleMeetSpawnDespawn`: presence, trigger matching and the build call become three pieces.
- **2d** `SpawnDriversFromFile`: parsing, roster filtering and spawn placement separated.
- **drive per step:** 2a — one spawn, one race, one despawn. 2b — a race to a finish. 2c — an ambient
  spawn *and* a manual one, then despawn. 2d — a meet with a full grid.
- **rule:** one stage per commit, and a stage never changes what a region does — only where it lives.

## Phase 3 — take the observation scaffolding out (no drive)

Timestamped lines, the tick heartbeat and the settings dump exist to observe the runtime; they are not the
shipped format and must not survive to release.

- remove `Log.I`, the heartbeat and the settings dump; keep `Log.Raw` as the only writer
- **keep the exception trap, re-routed through `Log.Raw`** (glm's caution, with one correction from the
  code: the trap already logs and **rethrows**, so removing it costs the evidence, not the crash
  semantics. Re-routing keeps both without the scaffolding.)
- re-read `docs/RUNTIME.md` against the final code and fix every number that moved

## Phase 4 — native wrappers (optional, feature-driven)

162 raw `Function.Call` sites. Not a work item, a tax: wrap a native only when a feature touches it. No
drive of its own.

## Stop conditions (glm's, adopted)

While the scaffolding log exists it is the oracle. **Stop and do not advance** on any of:

1. any exception line from the trap during a 2.x drive;
2. any state transition outside the shipped set in `docs/RUNTIME.md` — e.g. `NotStarted -> RaceInProgress`
   with no `CountDown` between them;
3. heartbeat gaps while the state is steady;
4. a settings dump after 1b that does not match the defaults table;
5. **meta:** any commit whose message cannot state *how* behaviour was established as unchanged.

## What each phase must leave behind

1. the tree builds with no new warnings, and the deployed DLL matches the build
2. `docs/RUNTIME.md` still true — if a number in it moved, the phase is not done
3. the reviewers' verdict on the change, and my own check recorded in the reply
4. a commit whose message says what behaviour did *not* change and how that was established
