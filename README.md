# Drag Meets — modern revival

The 2017 *Drag Meets* script, rebuilt so it runs on a current GTA V install.

The original was written against ScriptHookVDotNet v2 and last shipped as **2.2** in June 2017.
This repository keeps that code alive: same gameplay, same data, rebuilt against
`ScriptHookVDotNet2` 2.11.6, which the current loader still runs alongside v3.

## Why the source looks like this

The 2.2 source was never published. The repository's `master` branch stops at **1.9.4**, three
months and seven features behind the shipped build.

`src/DragMeets2.2/` was recovered by decompiling the shipped `DragMeets.dll` with ILSpy and
mechanically undoing the decompiler's artifacts (see `scripts/normalise-ilspy.py`). It is the
author's original logic; only the artifacts of decompilation were removed. The 2017 authored
sources are kept untouched in `vendor/eddlm-2017/` for reference.

## Layout

```
src/DragMeets2.2/     the working project (net48, C# 7.3, x64)
data/                 the shipped 2.2 data: config.ini, Meets/, Racers/
docs/PORT-REPORT.md   what had to change, and why
scripts/              recovery tooling
vendor/eddlm-2017/    the 2017 repository, untouched
```

## Build

```
cd src\DragMeets2.2
dotnet build
```

Output: `bin\Debug\DragMeets.dll`. The SDK-style project replaces the original toolchain, whose
reference path and post-build copy target both died with the 2017 machine.

## Install

Copy into the GTA V root:

```
scripts\DragMeets.dll
scripts\DragMeets\config.ini
scripts\DragMeets\Meets\*.xml
scripts\DragMeets\Racers\*.xml
```

The data folder is mandatory. The script's constructor enumerates `scripts\DragMeets\Meets` and
`scripts\DragMeets\Racers` at startup; without them it throws and never loads, with no visible
error. `scripts\DragMeets\debug.txt` is written on load and is the first thing to read.

`Meets\disabled\` holds meets that ship switched off. The loader uses a non-recursive
`Directory.GetFiles`, so anything in that subfolder stays off; move a file up one level to enable
it.

## Requirements

- GTA V (Legacy). Not tested on Enhanced.
- ScriptHookV.
- ScriptHookVDotNet: `ScriptHookVDotNet2.dll` (v2 API 2.11.6). The v3 runtime also works
  alongside it; the loader prints a deprecation warning for v2 scripts and runs them anyway.

## Known behaviour

- Meets are ambient: they spawn when you drive near a WaitingZone, and despawn when you leave.
- Racers with add-on car models that are not installed are skipped and logged, not fatal.
- No release builds yet.

## License

The 2017 repository carries no license file. Treat this as a private working copy until the
author decides otherwise.
