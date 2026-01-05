#!/usr/bin/env python3
"""
Script to migrate response DTOs from handler files to Contracts project.
"""

import os
import re
from pathlib import Path

def extract_response_records(file_path: Path):
    """Extract response record definitions from handler files."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"Error reading {file_path}: {e}")
        return []
    
    # Match response record definitions (not commands/queries)
    pattern = r'public record (\w+(?:Response|Dto))\s*\(([^)]+)\);'
    
    matches = re.finditer(pattern, content, re.MULTILINE | re.DOTALL)
    responses = []
    
    for match in matches:
        record_name = match.group(1)
        parameters = match.group(2)
        
        # Skip if it's a command or query
        if 'Command' in record_name or 'Query' in record_name:
            continue
        
        responses.append({
            'name': record_name,
            'parameters': parameters.strip(),
            'full_match': match.group(0)
        })
    
    return responses

def create_response_file(module_name: str, handler_path: str, response_info: dict, base_path: str):
    """Create a response DTO file in the Contracts project."""
    # Get the feature path
    rel_path = handler_path.split('/Features/v1/')[-1]
    parts = rel_path.split('/')[:-1]  # Remove Handler.cs filename
    
    # Create directory in Contracts
    contracts_base = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}.Contracts" / "v1"
    
    contract_dir = contracts_base
    for part in parts:
        contract_dir = contract_dir / part
    
    contract_dir.mkdir(parents=True, exist_ok=True)
    
    # Create the file
    file_name = f"{response_info['name']}.cs"
    file_path = contract_dir / file_name
    
    # If file exists, skip
    if file_path.exists():
        return None
    
    # Build namespace
    namespace_parts = ["FSH", "Module", module_name, "Contracts", "v1"] + list(parts)
    namespace = '.'.join(namespace_parts)
    
    # Build file content
    content = f"""namespace {namespace};

public sealed record {response_info['name']}({response_info['parameters']});
"""
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)
    
    return file_path

def update_handler_remove_response(file_path: Path, responses: list) -> bool:
    """Remove response definitions from handler file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Remove each response definition
        for resp in responses:
            pattern = re.escape(resp['full_match'])
            content = re.sub(pattern, '', content)
        
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

def main():
    """Main function."""
    base_path = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10"
    modules = ["Accounting", "Microfinance"]
    
    total_created = 0
    total_files = 0
    
    for module_name in modules:
        module_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}"
        features_path = module_path / "Features"
        
        if not features_path.exists():
            continue
        
        print(f"\n{'='*60}")
        print(f"Processing {module_name} module...")
        print(f"{'='*60}")
        
        handlers = list(features_path.rglob("*Handler.cs"))
        
        module_created = 0
        module_files = 0
        
        for handler in handlers:
            responses = extract_response_records(handler)
            
            if not responses:
                continue
            
            created_files = []
            for resp in responses:
                file_path = create_response_file(module_name, str(handler), resp, base_path)
                if file_path:
                    created_files.append((resp['name'], file_path))
                    module_created += 1
            
            if created_files:
                # Remove from handler
                if update_handler_remove_response(handler, responses):
                    module_files += 1
                    rel_path = handler.relative_to(module_path)
                    print(f"  ✓ {rel_path}")
                    for name, path in created_files:
                        print(f"    → Created {name}.cs")
        
        print(f"\n{module_name} Summary:")
        print(f"  Files updated: {module_files}")
        print(f"  Responses created: {module_created}")
        
        total_files += module_files
        total_created += module_created
    
    print(f"\n{'='*60}")
    print(f"TOTAL: {total_files} files updated, {total_created} responses created")
    print(f"{'='*60}")

if __name__ == "__main__":
    main()
