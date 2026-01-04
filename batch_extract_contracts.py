#!/usr/bin/env python3
"""
Batch Contract Extraction Script
Processes multiple features in sequence with verification

Usage:
    python3 batch_extract_contracts.py --features AccountingPeriods BankReconciliations DebitMemos
    python3 batch_extract_contracts.py --all --dry-run
    python3 batch_extract_contracts.py --batch Phase2E --verbose
"""

import os
import sys
import subprocess
import argparse
from pathlib import Path
from datetime import datetime

# Phase groupings for systematic extraction
PHASE_2E = [
    "AccountingPeriods",
    "BankReconciliations",
    "DebitMemos",
    "CreditMemos",
]

PHASE_2F = [
    "TaxRates",
    "TaxJournalEntries",
    "ExpenseCategories",
    "RevenueCategories",
]

PHASE_2G = [
    "SubLedgers",
    "GeneralLedgerAccounts",
    "AccountMappings",
    "DimensionValues",
]

# All remaining Phase 2 features (~100 features total)
ALL_PHASE_2 = PHASE_2E + PHASE_2F + PHASE_2G

class BatchExtractor:
    def __init__(self, dry_run: bool = False, verbose: bool = False):
        self.dry_run = dry_run
        self.verbose = verbose
        self.results = {}
        self.start_time = datetime.now()

    def log(self, msg: str):
        timestamp = datetime.now().strftime("%H:%M:%S")
        print(f"[{timestamp}] {msg}")

    def run_feature_extraction(self, feature: str) -> bool:
        """Run extraction for a single feature"""
        cmd = ["python3", "extract_contracts.py", "--feature", feature]
        
        if self.dry_run:
            cmd.append("--dry-run")
        if self.verbose:
            cmd.append("--verbose")
        
        try:
            self.log(f"⏳ Extracting {feature}...")
            result = subprocess.run(cmd, cwd=Path.cwd(), capture_output=True, timeout=120)
            
            success = result.returncode == 0
            self.results[feature] = {
                'success': success,
                'output': result.stdout.decode() if success else result.stderr.decode()
            }
            
            if success:
                self.log(f"✅ {feature} - COMPLETE")
            else:
                self.log(f"❌ {feature} - FAILED")
            
            return success
            
        except subprocess.TimeoutExpired:
            self.log(f"❌ {feature} - TIMEOUT")
            self.results[feature] = {'success': False, 'output': 'Timeout'}
            return False
        except Exception as e:
            self.log(f"❌ {feature} - ERROR: {e}")
            self.results[feature] = {'success': False, 'output': str(e)}
            return False

    def verify_build(self) -> bool:
        """Verify the build after extraction"""
        if self.dry_run:
            self.log("\n[DRY-RUN] Would verify build with: dotnet build FSH.Framework.slnx -c Debug")
            return True
        
        self.log("\n🔨 Verifying build...")
        try:
            result = subprocess.run(
                ["dotnet", "build", "FSH.Framework.slnx", "-c", "Debug"],
                cwd=Path("src").parent,
                capture_output=True,
                timeout=300
            )
            
            if result.returncode == 0:
                self.log("✅ Build verification PASSED")
                return True
            else:
                # Check if new errors were introduced
                output = result.stderr.decode()
                if "error" in output.lower():
                    self.log("❌ Build verification FAILED")
                    self.log("\nBuild errors:")
                    self.log(output[-2000:])  # Last 2000 chars
                    return False
                else:
                    self.log("⚠️  Build completed with warnings")
                    return True
        except Exception as e:
            self.log(f"❌ Build verification ERROR: {e}")
            return False

    def run_batch(self, features: list) -> int:
        """Run extraction for a batch of features"""
        self.log("=" * 70)
        self.log(f"🚀 BATCH CONTRACT EXTRACTION START")
        self.log(f"Features to process: {len(features)}")
        self.log(f"Dry-run: {self.dry_run}")
        self.log("=" * 70)
        
        success_count = 0
        failed_features = []
        
        for i, feature in enumerate(features, 1):
            self.log(f"\n[{i}/{len(features)}] Processing {feature}...")
            
            if self.run_feature_extraction(feature):
                success_count += 1
            else:
                failed_features.append(feature)
        
        # Verify build after all extractions
        if not self.dry_run and success_count > 0:
            self.verify_build()
        
        # Print summary
        self.print_summary(features, success_count, failed_features)
        
        return 0 if len(failed_features) == 0 else 1

    def print_summary(self, features: list, success_count: int, failed_features: list):
        """Print extraction summary report"""
        elapsed = datetime.now() - self.start_time
        
        self.log("\n" + "=" * 70)
        self.log("📊 BATCH EXTRACTION SUMMARY")
        self.log("=" * 70)
        self.log(f"Total features: {len(features)}")
        self.log(f"Successful: {success_count}")
        self.log(f"Failed: {len(failed_features)}")
        self.log(f"Time elapsed: {elapsed.total_seconds():.1f}s")
        
        if failed_features:
            self.log(f"\n❌ Failed features:")
            for feature in failed_features:
                self.log(f"  • {feature}")
        
        if self.dry_run:
            self.log("\n⚠️  DRY-RUN MODE: No actual changes were made")
        
        self.log("=" * 70)

def main():
    parser = argparse.ArgumentParser(
        description="Batch process multiple features for contract extraction",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  python3 batch_extract_contracts.py --features AccountingPeriods BankReconciliations
  python3 batch_extract_contracts.py --batch Phase2E --dry-run
  python3 batch_extract_contracts.py --batch Phase2E --verbose
  python3 batch_extract_contracts.py --all (Process all Phase 2 features)
        """
    )
    
    parser.add_argument(
        '--features',
        nargs='+',
        help='Specific features to process'
    )
    parser.add_argument(
        '--batch',
        choices=['Phase2E', 'Phase2F', 'Phase2G'],
        help='Pre-defined feature batch to process'
    )
    parser.add_argument(
        '--all',
        action='store_true',
        help='Process all Phase 2 features'
    )
    parser.add_argument(
        '--dry-run',
        action='store_true',
        help='Preview without making changes'
    )
    parser.add_argument(
        '--verbose',
        action='store_true',
        help='Show detailed output'
    )
    
    args = parser.parse_args()
    
    # Determine features to process
    features = []
    
    if args.features:
        features = args.features
    elif args.batch == 'Phase2E':
        features = PHASE_2E
    elif args.batch == 'Phase2F':
        features = PHASE_2F
    elif args.batch == 'Phase2G':
        features = PHASE_2G
    elif args.all:
        features = ALL_PHASE_2
    else:
        parser.print_help()
        return 1
    
    if not features:
        print("❌ No features specified")
        parser.print_help()
        return 1
    
    # Verify we're in the right directory
    if not Path("src/Modules/Accounting/Module.Accounting/Features").exists():
        print("❌ Error: Not in project root directory")
        sys.exit(1)
    
    # Run batch extraction
    extractor = BatchExtractor(dry_run=args.dry_run, verbose=args.verbose)
    return extractor.run_batch(features)

if __name__ == '__main__':
    sys.exit(main())
