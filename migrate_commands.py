#!/usr/bin/env python3
"""
Script to migrate command/query definitions from handlers to Contracts project.
Creates proper file structure in Contracts project and updates handlers to import them.
"""

import os
import re
from pathlib import Path
from typing import List, Tuple, Optional

def extract_command_info(file_path: Path) -> List[dict]:
    """Extract command/query definitions with their full context."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"Error reading {file_path}: {e}")
        return []
    
    # Match command/query definitions
    pattern = r'(?:///[^\n]*\n)*\s*public record (\w+(?:Command|Query))\(([^)]*)\)(?:\s*:\s*(ICommand<?[\w<>,\s]*>?|IQuery<[\w<>,\s]+>))?;'
    
    matches = re.finditer(pattern, content, re.MULTILINE | re.DOTALL)
    commands = []
    
    for match in matches:
        record_name = match.group(1)
        parameters = match.group(2)
        interface = match.group(3) if match.group(3) else "ICommand"
        
        # Extract the full definition including XML comments
        start = match.start()
        # Look back for XML comments
        lines_before = content[:start].split('\n')
        xml_lines = []
        for line in reversed(lines_before):
            if line.strip().startswith('///'):
                xml_lines.insert(0, line)
            elif line.strip():
                break
        
        xml_comments = '\n'.join(xml_lines) if xml_lines else ''
        full_definition = match.group(0).strip()
        
        commands.append({
            'name': record_name,
            'parameters': parameters,
            'interface': interface,
            'xml_comments': xml_comments,
            'full_definition': full_definition,
            'definition_start': start,
            'definition_end': match.end()
        })
    
    return commands

def create_contract_file(module_name: str, feature_path: str, command_info: dict, base_path: str):
    """Create a command/query file in the Contracts project."""
    # Determine the relative path from Features/v1/
    rel_path = feature_path.split('/Features/v1/')[-1]
    parts = rel_path.split('/')
    
    # Create directory structure in Contracts
    contracts_base = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}.Contracts" / "v1"
    
    # Build the directory path
    contract_dir = contracts_base
    for part in parts:
        contract_dir = contract_dir / part
    
    contract_dir.mkdir(parents=True, exist_ok=True)
    
    # Create the file
    file_name = f"{command_info['name']}.cs"
    file_path = contract_dir / file_name
    
    # Build namespace
    namespace_parts = ["FSH", "Module", module_name, "Contracts", "v1"] + list(parts)
    namespace = '.'.join(namespace_parts)
    
    # Build file content
    content = f"""using Mediator;

namespace {namespace};

public sealed record {command_info['name']}({command_info['parameters']}) : {command_info['interface']};
"""
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)
    
    return namespace

def update_handler_file(file_path: Path, commands: List[dict], namespaces: List[str]) -> bool:
    """Update handler file to remove commands and add imports."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Remove each command definition
        for cmd in commands:
            # Create pattern to match the full definition including XML comments
            pattern = r'(?:///[^\n]*\n)*\s*' + re.escape(cmd['full_definition'])
            content = re.sub(pattern, '', content, flags=re.MULTILINE)
        
        # Add using statements
        using_pattern = r'(using [^;]+;)\s*\n'
        matches = list(re.finditer(using_pattern, content))
        
        if matches and namespaces:
            last_match = matches[-1]
            insert_pos = last_match.end()
            
            new_usings = '\n'.join(f'using {ns};' for ns in sorted(set(namespaces)))
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
        print(f"Error updating {file_path}: {e}")
        return False

def process_handler_file(file_path: Path, module_name: str, base_path: str) -> int:
    """Process a single handler file."""
    commands = extract_command_info(file_path)
    
    if not commands:
        return 0
    
    # Get the feature path
    feature_path = str(file_path.parent)
    
    # Check if commands already exist in Contracts
    namespaces = []
    commands_to_create = []
    
    for cmd in commands:
        # Try to find existing command in Contracts
        contracts_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}.Contracts"
        existing_files = list(contracts_path.rglob(f"{cmd['name']}.cs"))
        
        if existing_files:
            # Extract namespace from existing file
            with open(existing_files[0], 'r', encoding='utf-8') as f:
                ns_match = re.search(r'namespace\s+([\w.]+);', f.read())
                if ns_match:
                    namespaces.append(ns_match.group(1))
        else:
            # Need to create the command file
            commands_to_create.append(cmd)
    
    # Create missing command files
    for cmd in commands_to_create:
        try:
            namespace = create_contract_file(module_name, feature_path, cmd, base_path)
            namespaces.append(namespace)
        except Exception as e:
            print(f"  Error creating contract for {cmd['name']}: {e}")
    
    # Update handler file
    if namespaces:
        if update_handler_file(file_path, commands, namespaces):
            return len(commands)
    
    return 0

def main():
    """Main function."""
    base_path = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10"
    modules = ["Accounting", "Microfinance"]
    
    total_fixed = 0
    total_created = 0
    
    for module_name in modules:
        module_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}"
        features_path = module_path / "Features"
        
        if not features_path.exists():
            print(f"Features path not found: {features_path}")
            continue
        
        print(f"\n{'='*60}")
        print(f"Processing {module_name} module...")
        print(f"{'='*60}")
        
        handlers = list(features_path.rglob("*Handler.cs"))
        print(f"Found {len(handlers)} handler files\n")
        
        module_fixed = 0
        module_created = 0
        
        for handler in handlers:
            count = process_handler_file(handler, module_name, base_path)
            if count > 0:
                module_fixed += 1
                module_created += count
                rel_path = handler.relative_to(module_path)
                print(f"  ✓ {rel_path} ({count} commands)")
        
        print(f"\n{module_name} Summary:")
        print(f"  Files updated: {module_fixed}")
        print(f"  Commands migrated: {module_created}")
        
        total_fixed += module_fixed
        total_created += module_created
    
    print(f"\n{'='*60}")
    print(f"TOTAL: {total_fixed} files updated, {total_created} commands migrated")
    print(f"{'='*60}")

if __name__ == "__main__":
    main()
