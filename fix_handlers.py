#!/usr/bin/env python3
"""
Script to fix handler files by removing duplicate command/query definitions
and adding proper imports from Contracts projects.
"""

import os
import re
from pathlib import Path
from typing import List, Tuple

def find_handler_files(module_path: str) -> List[Path]:
    """Find all handler files in the module."""
    handlers = []
    features_path = Path(module_path) / "Features"
    if features_path.exists():
        handlers = list(features_path.rglob("*Handler.cs"))
    return handlers

def extract_command_definition(content: str) -> Tuple[str | None, str | None]:
    """Extract command/query definition from handler file.
    Returns (definition, record_name) or (None, None) if not found.
    """
    # Match: public record CommandName(...) : ICommand<T> or IQuery<T>
    pattern = r'^(public record (\w+(?:Command|Query))\([^)]*\)(?:\s*:\s*(?:ICommand|IQuery)[^;]*)?;)'
    
    matches = re.finditer(pattern, content, re.MULTILINE)
    definitions = []
    
    for match in matches:
        full_def = match.group(1)
        record_name = match.group(2)
        definitions.append((full_def, record_name))
    
    return definitions

def find_contract_namespace(module_name: str, record_name: str, base_path: str) -> str | None:
    """Find the namespace where the command/query is defined in Contracts project."""
    contracts_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}.Contracts"
    
    if not contracts_path.exists():
        return None
    
    # Search for the command/query file
    for file_path in contracts_path.rglob(f"{record_name}.cs"):
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
                # Extract namespace
                ns_match = re.search(r'namespace\s+([\w.]+);', content)
                if ns_match:
                    return ns_match.group(1)
        except Exception as e:
            print(f"Error reading {file_path}: {e}")
            continue
    
    return None

def fix_handler_file(file_path: Path, module_name: str, base_path: str) -> bool:
    """Fix a single handler file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        definitions = extract_command_definition(content)
        
        if not definitions:
            return False
        
        # Track namespaces to import
        namespaces_to_import = set()
        
        # Remove each command/query definition
        for full_def, record_name in definitions:
            # Find the contract namespace
            contract_ns = find_contract_namespace(module_name, record_name, base_path)
            
            if contract_ns:
                namespaces_to_import.add(contract_ns)
                
                # Remove the definition (including any XML comments before it)
                # Pattern to match XML docs + definition
                pattern = r'(?:///[^\n]*\n)*\s*' + re.escape(full_def)
                content = re.sub(pattern, '', content, flags=re.MULTILINE)
        
        if not namespaces_to_import:
            return False
        
        # Add using statements after existing usings
        using_pattern = r'(using [^;]+;)\s*\n'
        
        new_usings = '\n'.join(f'using {ns};' for ns in sorted(namespaces_to_import))
        
        # Find last using statement and insert after it
        matches = list(re.finditer(using_pattern, content))
        if matches:
            last_match = matches[-1]
            insert_pos = last_match.end()
            # Add newline before namespace if needed
            remaining = content[insert_pos:].lstrip()
            content = content[:insert_pos] + new_usings + '\n\n' + remaining
        
        # Clean up extra blank lines
        content = re.sub(r'\n{3,}', '\n\n', content)
        
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            return True
        
        return False
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    """Main function to fix all handler files."""
    base_path = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10"
    modules = ["Accounting", "Microfinance"]
    
    total_fixed = 0
    
    for module_name in modules:
        module_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}"
        
        if not module_path.exists():
            print(f"Module path not found: {module_path}")
            continue
        
        print(f"\nProcessing {module_name} module...")
        handlers = find_handler_files(str(module_path))
        print(f"Found {len(handlers)} handler files")
        
        fixed_count = 0
        for handler in handlers:
            if fix_handler_file(handler, module_name, base_path):
                fixed_count += 1
                print(f"  Fixed: {handler.relative_to(module_path)}")
        
        print(f"Fixed {fixed_count} files in {module_name}")
        total_fixed += fixed_count
    
    print(f"\n=== Total: Fixed {total_fixed} handler files ===")

if __name__ == "__main__":
    main()
