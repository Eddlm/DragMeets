#!/usr/bin/env python3
"""Undo ILSpy artifacts in a decompiled tree so it compiles against ScriptHookVDotNet v2.

Usage:  python normalise-ilspy.py <tree>

Run it on the output of:

    ilspycmd -p -r <dir with ScriptHookVDotNet2.dll> -o <tree> DragMeets.dll

ILSpy 11.x emits six constructs that C# does not accept:

  1. file-scoped `namespace X;`
  2. `InputArgument.op_Implicit(x)` - operators cannot be called explicitly
  3. `Model.op_Implicit(x)`
  4. `(PedHash)(-123)` - negative constants cannot convert without `unchecked`
  5. `(Hash)(-123L)`
  6. `((Script)this).Dispose(dispose)`

It also leaves `Control` ambiguous once `System.Windows.Forms` and `GTA` are both imported.

Keep the project on C# 7.3: from C# 10 on, overload resolution for `InputArgument`'s implicit
operators changes and thousands of errors appear.
"""
import io
import os
import re
import sys


def unwrap_calls(text, key):
    """Remove a wrapper call, keeping its argument. (`op_Implicit(x)` -> `x`)"""
    out, i, count = [], 0, 0
    while True:
        j = text.find(key, i)
        if j < 0:
            out.append(text[i:])
            break
        out.append(text[i:j])
        k = j + len(key)
        depth, p = 1, k
        while p < len(text) and depth:
            if text[p] == '(':
                depth += 1
            elif text[p] == ')':
                depth -= 1
            p += 1
        out.append(text[k:p - 1])
        i, count = p, count + 1
    return ''.join(out), count


def fix_source(src):
    m = re.search(r'^namespace\s+([\w.]+)\s*;\s*$', src, re.M)
    if m:
        src = src[:m.start()] + 'namespace ' + m.group(1) + '\n{' + src[m.end():] + '\n}'
    for key in ('InputArgument.op_Implicit(', 'Model.op_Implicit('):
        src, _ = unwrap_calls(src, key)
    src = re.sub(r'\((PedHash|VehicleHash|WeaponHash)\)\((-\d+)\)',
                 r'unchecked((\1)(\2))', src)
    src = re.sub(r'\(Hash\)\((-\d+)L\)', r'(Hash)unchecked((ulong)(\1L))', src)
    src = src.replace('((Script)this).Dispose(dispose);', 'Dispose();')
    if ('using GTA.Native;' in src and 'using System.Windows.Forms;' in src
            and 'using Control = GTA.Control;' not in src):
        src = src.replace('using GTA.Native;',
                          'using GTA.Native;\nusing Control = GTA.Control;', 1)
    return src


def main(root):
    changed = 0
    for dirpath, _dirnames, filenames in os.walk(root):
        if os.sep + 'obj' in dirpath or os.sep + 'bin' in dirpath:
            continue
        for name in filenames:
            if not name.endswith('.cs'):
                continue
            path = os.path.join(dirpath, name)
            with io.open(path, encoding='utf-8-sig') as fh:
                original = fh.read()
            fixed = fix_source(original)
            if fixed != original:
                with io.open(path, 'w', encoding='utf-8') as fh:
                    fh.write(fixed)
                changed += 1
    print('files adjusted:', changed)


if __name__ == '__main__':
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    main(sys.argv[1])
