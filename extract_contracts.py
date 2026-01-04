#!/usr/bin/env python3
"""
Accounting Module Contract Extraction Automation Script
Extracts embedded commands/queries from handlers and creates contract files

Usage:
    python3 extract_contracts.py --feature InventoryItems
    python3 extract_contracts.py --feature AccountingPeriods --dry-run
    python3 extract_contracts.py --feature BankReconciliations --verbose
"""

import os
import sys
import re
import argparse
import shutil
from pathlib import Path
from typing import List, Tuple, Optional, Dict
import subprocess

# Configuration
FEATURES_PATH = "src/Modules/Accounting/Module.Accounting/Features/v1"
CONTRACTS_PATH = "src/Modules/Accounting/Module.Accounting.Contracts/v1"
MODULE_NAME = "FSH.Module.Accounting"

class ContractExtractor:
    def __init__(self, feature_name: str, dry_run: bool = False, verbose: bool = False):
        self.feature_name = feature_name
        self.dry_run = dry_run
        self.verbose = verbose
        self.features_dir = Path(FEATURES_PATH) / feature_name
        self.contracts_dir = Path(CONTRACTS_PATH) / feature_name
        self.operations = []
        self.updated_files = []
        self.created_files = []

    def log(self, msg: str, verbose_only: bool = False):
        if verbose_only and not self.verbose:
            return
        prefix = "[DRY-RUN] " if self.dry_run else ""
        print(f"{prefix}{msg}")

    def find_handlers(self) -> List[Path]:
        """Find all *Handler.cs files in the feature directory"""
        if not self.features_dir.exists():
            self.log(f"❌ Feature directory not found: {self.features_dir}")
            return []
        
        handlers = list(self.features_dir.glob("*/*Handler.cs"))
        self.log(f"Found {len(handlers)} handlers in {self.feature_name}", verbose_only=True)
        return handlers

    def extract_command_or_query(self, handler_content: str) -> Optional[Tuple[str, str]]:
        """
        Extract command/query record definition from handler file
        Returns: (record_definition, operation_name) or None
        """
        # Pattern for: public record CommandName(...) : ICommand<Type>;
        # Also handles multi-line records
        pattern = r'public record\s+(\w+)\([^)]*\)\s*:\s*I(?:Command|Query)[^;]*;'
        match = re.search(pattern, handler_content, re.DOTALL)
        
        if match:
            # Find the full record definition including all lines
            start_pos = handler_content.rfind('public record', 0, match.end())
            if start_pos == -1:
                start_pos = match.start()
            
            # Find the end semicolon
            end_pos = handler_content.find(';', match.start()) + 1
            if end_pos == 0:
                return None
            
            record_def = handler_content[start_pos:end_pos]
            operation_name = match.group(1)
            return (record_def, operation_name)
        
        return None

    def extract_xml_docs(self, handler_content: str, record_name: str) -> str:
        """Extract XML documentation comments above the record definition"""
        lines = handler_content.split('\n')
        xml_lines = []
        record_line_idx = -1
        
        # Find the record definition
        for i, line in enumerate(lines):
            if f'public record {record_name}' in line:
                record_line_idx = i
                break
        
        if record_line_idx == -1:
            return ""
        
        # Collect XML docs above the record
        for i in range(record_line_idx - 1, -1, -1):
            line = lines[i]
            if line.strip().startswith('///') or line.strip() == '':
                xml_lines.insert(0, line)
            else:
                break
        
        return '\n'.join(xml_lines)

    def extract_response_dtos(self, handler_content: str) -> str:
        """Extract response DTOs for GetList queries"""
        # Pattern for PagedResponse and Summary DTOs
        pattern = r'public record\s+(\w*(?:PagedResponse|SummaryDto))\([^)]*\)[^;]*;'
        matches = re.findall(pattern, handler_content, re.DOTALL)
        
        dto_defs = []
        for dto_name in matches:
            # Find full definition
            dto_pattern = rf'public record\s+{dto_name}\([^)]*\)[^;]*;'
            dto_match = re.search(dto_pattern, handler_content, re.DOTALL)
            if dto_match:
                dto_defs.append(dto_match.group(0))
        
        return '\n\n'.join(dto_defs)

    def extract_operation_dir(self, handler_path: Path) -> str:
        """Extract operation directory name from handler path"""
        # Path: Features/v1/FeatureName/OperationName/OperationNameHandler.cs
        return handler_path.parent.name

    def create_contract_file(self, operation_name: str, operation_dir: str, 
                            handler_path: Path) -> bool:
        """Create contract file for a command/query"""
        try:
            handler_content = handler_path.read_text()
            extraction = self.extract_command_or_query(handler_content)
            
            if not extraction:
                self.log(f"  ⚠️  No command/query found in {handler_path.name}", verbose_only=True)
                return False
            
            record_def, record_name = extraction
            xml_docs = self.extract_xml_docs(handler_content, record_name)
            response_dtos = self.extract_response_dtos(handler_content)
            
            # Determine if it's a Get/GetList query
            is_query = 'IQuery' in record_def
            operation_type = 'Get' if 'GetList' in operation_name or 'Get' in operation_name else operation_name
            
            # Build contract file content
            using_statements = "using Mediator;\n"
            
            # Add other using statements if needed
            if 'InventoryItemDto' in record_def or 'Dto' in record_def:
                using_statements = f"using {MODULE_NAME}.Contracts.v1.{self.feature_name};\nusing Mediator;\n"
            
            namespace = f"namespace {MODULE_NAME}.Contracts.v1.{self.feature_name}.{operation_dir};"
            
            content = f"""{using_statements}
{namespace}

{xml_docs}
{record_def}"""
            
            if response_dtos:
                content += f"\n\n{response_dtos}"
            
            # Create contract file
            contract_file = self.contracts_dir / operation_dir / f"{record_name}.cs"
            
            if self.dry_run:
                self.log(f"  📝 Would create: {contract_file}")
                return True
            
            contract_file.parent.mkdir(parents=True, exist_ok=True)
            contract_file.write_text(content)
            self.created_files.append(str(contract_file))
            self.log(f"  ✅ Created: {contract_file.name}")
            return True
            
        except Exception as e:
            self.log(f"  ❌ Error creating contract for {operation_name}: {e}")
            return False

    def update_handler_file(self, handler_path: Path, operation_dir: str, 
                           command_or_query_name: str) -> bool:
        """Update handler to reference contract and remove embedded command"""
        try:
            content = handler_path.read_text()
            original_content = content
            
            # Add using statement for contract
            using_stmt = f"using {MODULE_NAME}.Contracts.v1.{self.feature_name}.{operation_dir};"
            
            if using_stmt not in content:
                # Find the namespace line
                namespace_match = re.search(r'namespace\s+[^;]+;', content)
                if namespace_match:
                    insert_pos = namespace_match.start()
                    # Count existing using statements
                    using_lines = []
                    for line in content[:insert_pos].split('\n'):
                        if line.startswith('using'):
                            using_lines.append(line)
                    
                    # Add new using before namespace
                    content = content.replace('namespace', f'{using_stmt}\n\nnamespace', 1)
            
            # Remove embedded command/query record definition
            # Pattern: public record CommandName(...) : ICommand<...>;
            pattern = rf'public record\s+{command_or_query_name}\([^)]*\)\s*:\s*I(?:Command|Query)[^;]*;'
            content = re.sub(pattern, '', content, flags=re.DOTALL)
            
            # Also remove any surrounding XML documentation for that record
            xml_pattern = rf'(^\s*///.*?)*\s*public record\s+{command_or_query_name}'
            content = re.sub(xml_pattern, '', content, flags=re.MULTILINE | re.DOTALL)
            
            # Clean up extra blank lines
            content = re.sub(r'\n\n\n+', '\n\n', content)
            
            if content != original_content:
                if not self.dry_run:
                    handler_path.write_text(content)
                    self.updated_files.append(str(handler_path))
                self.log(f"  ✅ Updated handler: {handler_path.name}", verbose_only=True)
                return True
            else:
                self.log(f"  ⚠️  No changes needed for {handler_path.name}", verbose_only=True)
                return False
                
        except Exception as e:
            self.log(f"  ❌ Error updating handler {handler_path.name}: {e}")
            return False

    def update_validator_file(self, validator_path: Path, operation_dir: str,
                             command_or_query_name: str) -> bool:
        """Update validator to reference contract command"""
        try:
            if not validator_path.exists():
                return False
            
            content = validator_path.read_text()
            original_content = content
            
            # Add using statement
            using_stmt = f"using {MODULE_NAME}.Contracts.v1.{self.feature_name}.{operation_dir};"
            
            if using_stmt not in content:
                # Find first using or namespace
                namespace_match = re.search(r'namespace\s+[^;]+;', content)
                if namespace_match:
                    insert_pos = namespace_match.start()
                    content = content[:insert_pos] + f'{using_stmt}\n\n' + content[insert_pos:]
            
            if content != original_content:
                if not self.dry_run:
                    validator_path.write_text(content)
                    self.updated_files.append(str(validator_path))
                self.log(f"  ✅ Updated validator: {validator_path.name}", verbose_only=True)
                return True
                
        except Exception as e:
            self.log(f"  ❌ Error updating validator: {e}")
            return False

    def process_feature(self) -> bool:
        """Process all handlers in the feature"""
        self.log(f"\n🔄 Processing feature: {self.feature_name}")
        self.log("=" * 60)
        
        handlers = self.find_handlers()
        if not handlers:
            self.log(f"❌ No handlers found for {self.feature_name}")
            return False
        
        success_count = 0
        
        for handler_path in handlers:
            operation_dir = self.extract_operation_dir(handler_path)
            self.log(f"\n📋 Operation: {operation_dir}")
            
            # Extract command/query
            handler_content = handler_path.read_text()
            extraction = self.extract_command_or_query(handler_content)
            
            if not extraction:
                self.log(f"  ⚠️  No command/query found")
                continue
            
            record_def, record_name = extraction
            
            # Create contract file
            if self.create_contract_file(record_name, operation_dir, handler_path):
                # Update handler
                if self.update_handler_file(handler_path, operation_dir, record_name):
                    # Update validator if exists
                    validator_path = handler_path.parent / f"{record_name.replace('Command', '').replace('Query', '')}Validator.cs"
                    if validator_path.exists():
                        self.update_validator_file(validator_path, operation_dir, record_name)
                    
                    success_count += 1
                    self.operations.append(record_name)
        
        return success_count > 0

    def run_formatter(self) -> bool:
        """Run dotnet format on the affected modules"""
        if self.dry_run:
            self.log("\n[DRY-RUN] Would run: dotnet format src/Modules/Accounting/")
            return True
        
        try:
            self.log("\n🎨 Running code formatter...")
            result = subprocess.run(
                ["dotnet", "format", "src/Modules/Accounting/"],
                cwd=Path.cwd(),
                capture_output=True,
                timeout=60
            )
            if result.returncode == 0:
                self.log("✅ Code formatting complete")
                return True
            else:
                self.log(f"⚠️  Formatting completed with warnings")
                return True  # Don't fail on formatting
        except Exception as e:
            self.log(f"⚠️  Could not run formatter: {e}")
            return True

    def report(self):
        """Print extraction report"""
        self.log("\n" + "=" * 60)
        self.log("📊 EXTRACTION REPORT")
        self.log("=" * 60)
        self.log(f"Feature: {self.feature_name}")
        self.log(f"Operations extracted: {len(self.operations)}")
        self.log(f"Files created: {len(self.created_files)}")
        self.log(f"Files updated: {len(self.updated_files)}")
        
        if self.operations:
            self.log(f"\nExtracted operations:")
            for op in self.operations:
                self.log(f"  • {op}")
        
        if self.dry_run:
            self.log("\n⚠️  DRY-RUN MODE: No changes were made")

def main():
    parser = argparse.ArgumentParser(
        description="Extract embedded commands/queries from Accounting module handlers",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  python3 extract_contracts.py --feature InventoryItems
  python3 extract_contracts.py --feature AccountingPeriods --dry-run
  python3 extract_contracts.py --feature BankReconciliations --verbose
        """
    )
    
    parser.add_argument(
        '--feature',
        required=True,
        help='Feature name (e.g., InventoryItems, AccountingPeriods)'
    )
    parser.add_argument(
        '--dry-run',
        action='store_true',
        help='Preview changes without modifying files'
    )
    parser.add_argument(
        '--verbose',
        action='store_true',
        help='Show detailed output'
    )
    parser.add_argument(
        '--no-format',
        action='store_true',
        help='Skip running dotnet format'
    )
    
    args = parser.parse_args()
    
    # Change to project root if needed
    if not Path(FEATURES_PATH).exists():
        print(f"❌ Error: {FEATURES_PATH} not found")
        print("Make sure to run this script from the project root directory")
        sys.exit(1)
    
    # Run extractor
    extractor = ContractExtractor(
        feature_name=args.feature,
        dry_run=args.dry_run,
        verbose=args.verbose
    )
    
    success = extractor.process_feature()
    
    if success and not args.no_format and not args.dry_run:
        extractor.run_formatter()
    
    extractor.report()
    
    return 0 if success else 1

if __name__ == '__main__':
    sys.exit(main())
