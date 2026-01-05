#!/usr/bin/env python3
import re
from pathlib import Path

BASE = Path('/Users/kirkeypsalms/Projects/dotnet-starter-kit-10')
MODULE = 'Accounting'

# 1) Fix command signatures for handlers that expect Unit (ICommandHandler<T>)
handlers = list((BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}' / 'Features').rglob('*Handler.cs'))
changed = []
for h in handlers:
    text = h.read_text()
    # find class declaration with ICommandHandler<CommandName>
    m = re.search(r':\s*ICommandHandler<\s*(\w+)\s*>', text)
    if not m:
        continue
    cmd = m.group(1)
    # check if Handle returns ValueTask<Unit>
    if 'ValueTask<Unit>' in text or 'ValueTask<Unit>' in text:
        # find contract file for that command
        contract_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}.Contracts'
        candidates = list(contract_root.rglob(f'{cmd}.cs'))
        if not candidates:
            continue
        cf = candidates[0]
        ctext = cf.read_text()
        # replace ': ICommand<...>' with ': ICommand' if necessary
        if re.search(rf':\s*ICommand<', ctext):
            new = re.sub(r':\s*ICommand<[^>]+>', ': ICommand', ctext)
            cf.write_text(new)
            changed.append(str(cf.relative_to(BASE)))

# 2) Add missing using directives to validators
validators = list((BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}' / 'Features').rglob('*Validator.cs'))
validator_changes = []
for v in validators:
    text = v.read_text()
    m = re.search(r'AbstractValidator<\s*(\w+Command)\s*>', text)
    if not m:
        continue
    cmd = m.group(1)
    # find contract file for cmd
    contract_root = BASE / 'src' / 'Modules' / MODULE / f'Module.{MODULE}.Contracts'
    candidates = list(contract_root.rglob(f'{cmd}.cs'))
    if not candidates:
        continue
    cf = candidates[0]
    # derive namespace from contract file
    content = cf.read_text()
    ns = re.search(r'namespace\s+([\w.]+);', content)
    if not ns:
        continue
    contract_ns = ns.group(1)
    # if using directive missing, add it after existing usings
    if f'using {contract_ns};' not in text:
        # find last using
        m2 = list(re.finditer(r'(using [^;]+;)', text))
        insert = 0
        if m2:
            insert = m2[-1].end()
            newtext = text[:insert] + '\n' + f'using {contract_ns};' + text[insert:]
        else:
            # add at top
            newtext = f'using {contract_ns};\n' + text
        v.write_text(newtext)
        validator_changes.append(str(v.relative_to(BASE)))

print('Command files updated:')
for c in changed:
    print(' -', c)
print('Validator files updated:')
for c in validator_changes:
    print(' -', c)

a = len(changed) + len(validator_changes)
print('\nTotal files updated:', a)
