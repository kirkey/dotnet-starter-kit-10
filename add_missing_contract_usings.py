#!/usr/bin/env python3
import re
from pathlib import Path

BASE = Path('/Users/kirkeypsalms/Projects/dotnet-starter-kit-10')
MODULE = 'Accounting'

features_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}' / 'Features'
contracts_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}.Contracts'

files = list(features_root.rglob('*.cs'))
updated = []

# build map of command/query name -> namespace
contract_files = list(contracts_root.rglob('*.cs'))
name_to_ns = {}
for cf in contract_files:
    content = cf.read_text()
    m = re.search(r'namespace\s+([\w.]+);', content)
    if not m:
        continue
    ns = m.group(1)
    # try to extract the public record name at top
    m2 = re.search(r'public\s+(?:sealed\s+)?record\s+(\w+)', content)
    if m2:
        name_to_ns[m2.group(1)] = ns

pattern = re.compile(r'\b([A-Z][A-Za-z0-9]+(?:Command|Query|Dto|Response))\b')

for f in files:
    text = f.read_text()
    matches = set(pattern.findall(text))
    usings_to_add = set()
    for match in matches:
        if match in name_to_ns:
            ns = name_to_ns[match]
            # Only add contract namespaces (that include .Contracts.) and not the module root
            if '.Contracts.' in ns:
                if f'using {ns};' not in text:
                    usings_to_add.add(ns)
    if usings_to_add:
        # Insert after last using
        m = list(re.finditer(r'(using [^;]+;)', text))
        if m:
            insert_at = m[-1].end()
            addition = '\n'.join(f'using {ns};' for ns in sorted(usings_to_add))
            newtext = text[:insert_at] + '\n' + addition + text[insert_at:]
        else:
            newtext = '\n'.join(f'using {ns};' for ns in sorted(usings_to_add)) + '\n\n' + text
        f.write_text(newtext)
        updated.append(str(f.relative_to(BASE)))

print('Files updated with contract usings:')
for u in updated:
    print(' -', u)
print('\nTotal files updated:', len(updated))
