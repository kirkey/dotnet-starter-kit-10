#!/usr/bin/env python3
"""
Script to add response definitions to query files from handler files.
"""

import os
import re
from pathlib import Path

def process_query_and_handler(query_file: Path, handler_file: Path):
    """Add response definition from handler to query file."""
    try:
        # Read handler to find response
        with open(handler_file, 'r', encoding='utf-8') as f:
            handler_content = f.read()
        
        # Extract response definition
        response_pattern = r'public record (\w+Response)\s*\(([^)]+)\);'
        match = re.search(response_pattern, handler_content)
        
        if not match:
            return False
        
        response_name = match.group(1)
        response_params = match.group(2).strip()
        
        # Read query file
        with open(query_file, 'r', encoding='utf-8') as f:
            query_content = f.read()
        
        # Check if response already exists in query
        if response_name in query_content:
            return False
        
        # Add response definition after query definition
        response_def = f"\npublic sealed record {response_name}({response_params});"
        
        # Find end of query namespace
        lines = query_content.split('\n')
        insert_index = -1
        
        for i, line in enumerate(lines):
            if line.strip() and not line.strip().startswith('using') and 'Query' in line:
                # Found query, insert after it
                insert_index = i + 1
                break
        
        if insert_index > 0:
            lines.insert(insert_index, response_def)
            new_content = '\n'.join(lines)
            
            with open(query_file, 'w', encoding='utf-8') as f:
                f.write(new_content)
            
            return True
        
        return False
        
    except Exception as e:
        print(f"Error processing {query_file}: {e}")
        return False

def main():
    """Main function."""
    base_path = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10"
    
    # List of query files that need response definitions
    queries_needing_responses = [
        ("Accounting", "Members/GetListMember"),
        ("Accounting", "Meters/GetListMeter"),
        ("Accounting", "AccountsReceivable/GetListAccountsReceivableAccount"),
        ("Accounting", "SecurityDeposits/GetListSecurityDeposit"),
        ("Accounting", "PrepaidExpenses/GetListPrepaidExpense"),
        ("Accounting", "InterconnectionAgreements/GetListInterconnectionAgreement"),
        ("Accounting", "DepreciationMethods/GetListDepreciationMethod"),
        ("Accounting", "RegulatoryReports/GetListRegulatoryReport"),
    ]
    
    fixed = 0
    
    for module_name, feature_path in queries_needing_responses:
        contracts_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}.Contracts" / "v1" / feature_path
        module_path = Path(base_path) / "src" / "Modules" / module_name / f"Module.{module_name}" / "Features" / "v1" / feature_path
        
        # Find query file in contracts
        query_files = list(contracts_path.glob("*Query.cs"))
        if not query_files:
            print(f"No query file found in {contracts_path}")
            continue
        
        query_file = query_files[0]
        
        # Find handler file in module
        handler_files = list(module_path.glob("*Handler.cs"))
        if not handler_files:
            print(f"No handler file found in {module_path}")
            continue
        
        handler_file = handler_files[0]
        
        if process_query_and_handler(query_file, handler_file):
            print(f"✓ Added response to {query_file.relative_to(base_path)}")
            fixed += 1
    
    print(f"\nTotal: {fixed} query files updated")

if __name__ == "__main__":
    main()
