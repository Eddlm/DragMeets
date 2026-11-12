# How Drag Meets runs at runtime

A high-level description of what the script does while the game is running. It deliberately names no
methods, files or line numbers of its own: the code beneath it is being simplified, and this document
is meant to survive that. Where a number appears, it was measured, not estimated.

Measured on a session of 2026-11-12: four races started, three finished, one abandoned mid-race, six
states observed, no exceptions.

## The subject: one meet at a time

A *meet* is an ambient drag race event. When the player drives into a waiting zone, a meet is chosen,
constructed and placed in the world: a start line, a finish line, a grid of racers, a crowd, and props.
It lives near the player and goes away again when the player leaves. In the measured session exactly one
meet was loaded at any moment; the code asks whether *any* meet is loaded, so more than one may be
possible in principle — unverified.

A racer is a car with a driver, drawn from a roster file, given a tuning and a skill level. The player
can join the grid as a participant or stand aside and watch the AI run the race.

## The lifecycle

| State | What it means | Measured time |
|---|---|---|
| `NotStarted` | nothing is loaded; the script is watching the player's position | until first meet (59.5 s in the measured session) |
| `Idle` | a meet is being chosen and read from disk | 1.2–2.0 s |
| `Setup` | the world is being built: racers spawned and tuned, crowd and props placed | 25.4–32.1 s |
| `CountDown` | the grid is closed and waiting for the start signal | 5.1–5.2 s |
| `RaceInProgress` | racing, with finish detection running | 18.7–25.0 s over 582 m |
| `RaceFinished` | the order is recorded | 0.5 s |

Then back to `Idle`, and a new meet can begin within a second or two — meets cycle while the player stays
in the area.

The start is not the state transition. `CountDown` ends when the flare ped fires; the start line event
and the state change are two separate moments inside the same window.

`Setup` is the only expensive state. Spawning a full grid is the one place frame rate dips in the whole
session: roughly 62 ticks/s to 31 ticks/s for about five seconds. On a manual start this is hidden by
the fadeout, so it is a cost to know about, not a defect to chase.

Leaving the area does **not** unwind the meet. Mid-race, the state jumps straight to `NotStarted`, the
meet is no longer loaded, and everything it was holding is dropped. Measured: `RaceInProgress` to
`NotStarted` in one step, skipping `RaceFinished` and `Idle` entirely. The exit path therefore cannot be
reasoned about as the reverse of setup.

## The pieces

- **Racers** exist before the race and are picked to participate; each carries a car, a driver, a
tuning and a skill level. Racers can be removed between races and added again.
- **The player** is either a participant or a spectator; the race runs to completion either way.
- **Spectators and crowd** stand in crowd areas and react to the cars; they are placed during `Setup`
  and animated independently of the race.
- **Props** are the start line, the finish barrel, the flares and the flare ped whose shot starts the race.

## Per-frame work

The tick runs on every game frame. Read in order, its responsibilities are:

1. ambient chatter and notifications;
2. player input — cheat strings and key handlers;
3. meet presence: is a meet loaded, should one be spawned or dropped, given where the player is;
4. the state machine: what the current state does on this frame;
5. race supervision: who is participating, who has finished, who is stuck.

On an idle frame this costs almost nothing (about 62 ticks per second); the cost concentrates in 3 and 4
and in the spawn work inside `Setup`.

## Timers, and why things happen out of state

Two periodic timers run beside the state machine, on their own intervals: one tops up the grid while the
meet is under its racer cap (a coin toss per interval decides whether to add one) and keeps the crowd
moving; the other does lighter maintenance on a shorter interval.

Because they are timer-driven, they fire in **any** state — a racer can be added to a meet that is
already racing. That is intended behaviour ("fill the drag"), not a scheduling bug, and it explains log
lines about spawning that appear in the middle of a race.

## Data on disk

- `config.ini` — the tuning surface: racer cap, blips, designs, money ranges, intervals, and the rest of
  the 38 tunables. The shipped file is the contract; change a value there, not in code.
- `Meets/*.xml` — one file per meet: waiting zone, start and finish geometry, crowd areas, spawn nodes.
  Meets in the `disabled` subfolder are deliberately not loaded, because enumeration is not recursive.
- `Racers/*.xml` — the roster: names, car models, tunings.

Meet files are read when a meet is chosen. The data folder is mandatory: without it the script throws
before it can log anything useful.

## The log

`debug.txt` is truncated on every load; its first line is the load proof, carrying the game version.
Two kinds of line now sit in it: the shipped ones, kept byte-compatible so sessions stay comparable with
the 2.2 build, and the scaffolding added for observation (timestamped state and tick lines, and a dump
of the settings in effect). The scaffolding is temporary and must be removed before release.

## Hidden couplings

**The sleeper engine.** While a meet is being built, up to four non-player racers in the slow classes
(compacts, sedans, SUVs, coupes, vans) are candidates for a build-up. For each one a coin toss decides
between two branches: fit a visible engine prop (`prop_car_engine_01`, attached to the engine bone,
announced as *"has EMS5"*) or leave the car looking stock (a *"sleeper"*).

Both branches write the same integer, `CustomTurbo`, and that integer is added straight into the car's
engine-power multiplier: **1** by default, **20-100** for a sleeper, **200** for the visible build. The
prop is decoration; the number is the physics.

What this couples to, before anyone touches it:

- the car's `PerformancePoints` are computed from the car itself, never from `CustomTurbo`, so the
  betting and pairing logic reads a juiced car as slow — by design or by oversight, unverified;
- the class filter is what makes it a sleeper: only the slow classes qualify, and the player never does;
- a roster entry can arrive pre-juiced, because the integer is read from `Racers.xml`;
- the routine that does all this is named after its losing branch and performs both jobs.

## Open questions

Each of these is a gap in the description above, not a task:

- **What decides a despawn.** Distance from the zone, the zone's own geometry, or something else. The
  last transition still without a rule.
- **Why the despawn lands on `NotStarted`** rather than a teardown path, and whether anything it held
  leaks — a car, a ped, a blip.
- **What `Setup` spends its 25–32 seconds on**: racer construction, file parsing, or fade sequencing.
- **Whether a grid larger than two changes the finish accounting.** Every race observed had exactly two
  participants.
