#!/usr/bin/env python3
from pathlib import Path
import re

BASE = Path('/Users/kirkeypsalms/Projects/dotnet-starter-kit-10')
MODULE = 'Accounting'
features_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}' / 'Features'
contracts_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}.Contracts'

# Build map of type name -> list of namespaces
name_to_ns = {}
for cf in contracts_root.rglob('*.cs'):
    txt = cf.read_text()
    mns = re.search(r'namespace\s+([\w.]+);', txt)
    mname = re.search(r'public\s+(?:sealed\s+)?record\s+(\w+)', txt)
    if mns and mname:
        name = mname.group(1)
        ns = mns.group(1)
        name_to_ns.setdefault(name, []).append(ns)

updated = []
# For each feature file, check for ambiguous types
for f in features_root.rglob('*.cs'):
    text = f.read_text()
    # find all type names used that have multiple namespaces
    ambiguous = []
    for name, nss in name_to_ns.items():
        if len(nss) > 1 and re.search(r'\b' + re.escape(name) + r'\b', text):
            ambiguous.append((name, nss))
    if not ambiguous:
        continue
    new_text = text
    changed = False
    # Compute feature path suffix to help decide which namespace to keep
    rel = f.relative_to(features_root)
    parts = rel.parts
    # part[0] is top-level entity name, part[1] is the feature folder like GetListX or GetX
    feature_key = None
    if len(parts) >= 2:
        feature_key = f"{parts[0]}.{parts[1]}"
    for name, nss in ambiguous:
        # find candidate ns that best matches file path
        chosen = None
        for ns in nss:
            # if namespace ends with the specific feature folder that matches file's path, choose it
            if feature_key and feature_key.replace('.', '\\.') in ns:
                chosen = ns
                break
        if not chosen:
            # fallback: if one namespace contains 'GetList' and file path contains 'GetList', choose it
            for ns in nss:
                if 'GetList' in ns and 'GetList' in str(rel):
                    chosen = ns
                    break
        if not chosen:
            # pick the ns that is a direct child of contracts (less likely)
            chosen = nss[0]
        # Remove other conflicting 'using' statements
        for ns in nss:
            if ns == chosen:
                continue
            pattern = rf'\nusing\s+{re.escape(ns)};'
            if re.search(pattern, new_text):
                new_text = re.sub(pattern, '', new_text)
                changed = True
    if changed:
        # clean up extra blank lines
        new_text = re.sub(r'\n{3,}', '\n\n', new_text)
        f.write_text(new_text)
        updated.append(str(f.relative_to(BASE)))

print('Files updated to remove ambiguous usings:')
for u in updated:
    print(' -', u)
print('\nTotal:', len(updated))
