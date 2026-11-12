# Drag Meets 2.2 — port to a 2026 GTA V Legacy install

What was required to take Eddlm's *Drag Meets* (2017, ScriptHookVDotNet v2) from a dead repo to a
running mod on GTA V Legacy, GameVer 103, SHVDN 2.11.6 under the Enhanced-capable loader 3.9.0.6.

Result: script loads, meets spawn, racers spawn and the AI cycle runs. Verified in-game by the
user; log evidence in `scripts\DragMeets\debug.txt`.

## 0. The starting point was worse than "old code"

- GitHub HEAD (`0006a13`, 2017-03-20) is **1.9.4**. The shipped release is **2.2** (June 2017).
  Seven shipped features (drag parachutes, per-class racer money, Lamar notifications, private
  meets, activity notifications, guides, sleepers) exist **only in the binary**.
- The repo contains three copies of the sources and **no data at all** — no `config.ini`, no
  meets, no racer pool. The script's constructor calls `Directory.GetFiles` on a folder that
  would not exist, so a repo-only build throws and never instantiates: the classic "the mod does
  nothing, no error".

## 1. Source recovery

- Release package fetched from `files.gta5-mods.com` (164,632 B), extracted and verified:
  22 active meets (12 more in `disabled\`), `Racers.xml` with 274 drivers, `config.ini` with 39
  keys, `Player.xml` as a 53-byte stub the script overwrites.
- The package carries **two builds** of the same DLL: the original and a temporary
  ManualGears/Gunrunning fix. Both 118,784 B, different hashes. Decompiling both and diffing the
  normalised trees shows the fix is **three call sites** in `Racer.cs` — `Util.CurrentGear(Car)`
  (a raw read at `Entity.MemoryAddress + 2032`) in place of the managed `Car.CurrentGear`.
  The original is the correct base on a current build; the patch was for ManualGears, which is
  off here (`MT_Support=false`).
- 2.2 decompiled with `ilspycmd` **11.2** (`-p -r refs`, reference = `ScriptHookVDotNet2.dll`).
  9.1 emits invalid `(Vector3)(ref field)` casts and is unusable.

## 2. Decompiler artifacts, fixed mechanically

| artifact | sites | fix |
|---|---|---|
| file-scoped `namespace X;` | 4 files | block-scoped namespace, usings preserved |
| `InputArgument.op_Implicit(x)` | 466 | unwrapped to `x` |
| `Model.op_Implicit(x)` | 65 | unwrapped to `x` |
| `(PedHash)(-123)` etc. | 5 | `unchecked((PedHash)(-123))` |
| `((Script)this).Dispose(dispose)` | 1 | `Dispose()` |
| `Control` ambiguous (WinForms vs `GTA.Control`) | 2 | `using Control = GTA.Control;` |

**Language version matters:** the project must stay on **C# 7.3**. C# 10 changed overload
resolution for `InputArgument`'s implicit operators and turns 5 errors into 606.

## 3. Project

New SDK-style csproj (`src\DragMeets2.2\`): `net48`, `LangVersion 7.3`, `PlatformTarget x64`,
`UseWindowsForms`, `AllowUnsafeBlocks`, `GenerateAssemblyInfo false`, and
`PackageReference ScriptHookVDotNet2 2.11.6`. This replaces the original toolchain entirely:

- the original `HintPath` pointed at `..\..\ScriptHook_VDotNet\ScriptHookVDotNet.dll`, a path that
  does not exist anywhere;
- the original `PostBuildEvent` copied to `D:\Juegos\Steam\...\scripts`, a 2017 machine.

Both were bypassed rather than repaired. Reference identity matches the installed repack exactly
(`ScriptHookVDotNet2, Version=2.11.6.0`), so binding is safe despite the files differing in size.

## 4. What did **not** need changing

- **The 167 `Function.Call` sites, 105 distinct natives.** Compiling against the current v2
  package proves every named member exists; raw `(Hash)<number>` sites are stable across game
  builds. No rename audit was needed and none was done.
- **No SHVDN v2 → v3 port.** The Enhanced-capable loader 3.9.0.6 still loads `ScriptHookVDotNet2`
  alongside `ScriptHookVDotNet3`; v2 scripts run with a deprecation warning. Target v2 first was
  the right call.
- **No menu library.** The mod's `UI.Notify` resolves to SHVDN2's own `GTA.UI`; `NativeUI.dll`
  is present in `scripts\` but is not a dependency of this mod (0 references in both 1.9.4 and
  2.2 binaries).
- **No memory hacks in the path.** `Util.CurrentGear` is dead code in the original build.

## 5. Data and config corrections (author's bugs, not the port's)

- `config.ini`: `CinematicCamera = true` → `CinematicCam = true`. The loader reads
  `[MEETS] CinematicCam`; the shipped key name never matched, so the toggle did nothing (the
  code default is `true`, which is why nobody noticed).
- `config.ini`: `ManualMeetSpawn=true` → `false`. With `true`, meets never auto-spawn; the code
  default is `false` and the author had overridden it. Set to `false` for the ambient cycle.
- `Meets\San Chianski.xml`: a third `<AmbientVehicleParkZone>` at `(-909,-3096,13)` — the LS
  Airport coordinate, ~4 km from the meet — deleted.
- `Meets\Popular St..xml`: two `<CrowdArea>` blocks duplicated byte-for-byte; duplicates removed.
- Left alone deliberately: `PoliceDisbandsMeets` is read by nothing (the ini itself says "not
  implemented yet"); `Racers - original.xml` / `Player - original.xml` are the author's own
  backups.

## 6. The one code change

`DragMeets.cs`, in `HandleMeetSpawnDespawn`'s spawn loop: added `if (IsAnyMeetLoaded()) break;`
before the spawn. 2.2 evaluates "is a meet loaded" once, outside the loop, then spawns from
inside it — so two meets whose WaitingZones fall in the same 300 m radius both start.
`MountainBesidesTrevorsAirfield` and `the Sandy Shores Airfield` are **297.8 m** apart, the only
pair in the map under the radius, which is why exactly two meets stacked.

## 7. Deployment

```
scripts\DragMeets.dll                  rebuilt, 128,000 B
scripts\DragMeets\config.ini
scripts\DragMeets\Meets\*.xml          22 active (+12 in disabled\, not loaded)
scripts\DragMeets\Racers\Racers.xml    274 drivers
scripts\DragMeets\Racers\Player.xml    runtime stub
```

`Directory.GetFiles` is called without `SearchOption`, so `disabled\` stays off — that is what
keeps the 12 extra meets dark. Changing that call to `AllDirectories` turns them all on.

## 8. Evidence of a working port

`scripts\DragMeets\debug.txt` from the first live session:

```
Script started -11/12/2026 11:48:21 AM-GameVer 103
Matching MeetFile found. Loading scripts\DragMeets\Meets\MountainBesidesTrevorsAirfield.xml...
All meet info loaded. - Spawning cars - ... - Meet loaded. Have fun.
```

`ScriptHookVDotNet.log`: `Found 1 script(s) in DragMeets.dll resolved to API version 2.11.6`,
`Instantiating script Drag_Meets_Reborn.DragMeets`, `Started script`.

Expected-but-harmless noise in that log: `NO MEET TRIGGER FOUND, looking for WaitingZone` (13 of
the 22 meets define no `<Trigger>`; the code falls back to the WaitingZone by design) and
`ERROR vehicle model not found in game files, SKIPPED` (racer entries naming add-on cars).

## Build and deploy

```
cd D:\Projects\GTA\DragMeets\src\DragMeets2.2
dotnet build
copy bin\Debug\DragMeets.dll "<GTA V>\scripts\DragMeets.dll"
```
