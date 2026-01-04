#!/usr/bin/env python3
"""
Phase 2E-Z Automation Master Script

Orchestrates the complete extraction and verification workflow for Phase 2E-Z.
This script manages the extraction of ~255 command/query contracts across 
accounting module handlers in phases.

Phase 2E-Z includes:
  - Phase 2E: 31 operations (4 features)
  - Phase 2F: 25 operations (4 features)
  - Phase 2G: 22 operations (4 features)
  - Phase 2H-Z: 193+ operations (80+ features)

Usage:
    python3 run_phase_2ez.py [--phase PHASE] [--all] [--dry-run] [--verbose]

Examples:
    python3 run_phase_2ez.py --phase Phase2E
    python3 run_phase_2ez.py --all
    python3 run_phase_2ez.py --phase Phase2E --dry-run
    python3 run_phase_2ez.py --all --verbose
"""

import os
import sys
import subprocess
import argparse
import json
from pathlib import Path
from datetime import datetime
from typing import List, Dict, Optional, Tuple


class Phase2EZOrchestrator:
    """Master orchestrator for Phase 2E-Z extraction automation."""

    # Phase definitions with operation counts
    PHASES = {
        "Phase2E": {
            "features": [
                "AccountingPeriods",
                "BankReconciliations",
                "DebitMemos",
                "CreditMemos",
            ],
            "operations": 31,
            "description": "Core accounting period and reconciliation operations",
        },
        "Phase2F": {
            "features": [
                "TaxRates",
                "TaxJournalEntries",
                "ExpenseCategories",
                "RevenueCategories",
            ],
            "operations": 25,
            "description": "Tax and categorization operations",
        },
        "Phase2G": {
            "features": [
                "SubLedgers",
                "GeneralLedgerAccounts",
                "AccountMappings",
                "DimensionValues",
            ],
            "operations": 22,
            "description": "Ledger and mapping operations",
        },
    }

    LARGE_PHASES = {
        "Phase2H-Z": {
            "features": [],  # Will be populated dynamically
            "operations": 193,
            "description": "Remaining accounting module features",
        },
    }

    def __init__(self, dry_run: bool = False, verbose: bool = False):
        """Initialize the orchestrator.
        
        Args:
            dry_run: Preview changes without modifying files
            verbose: Show detailed processing output
        """
        self.dry_run = dry_run
        self.verbose = verbose
        self.base_path = Path("/Users/kirkeypsalms/Projects/dotnet-starter-kit-10")
        self.start_time = datetime.now()
        self.results = {}
        self.total_operations = 0

    def print_header(self) -> None:
        """Print welcome header."""
        print("\n" + "=" * 70)
        print("🚀 PHASE 2E-Z AUTOMATION MASTER SCRIPT")
        print("=" * 70)
        print(f"Start time: {self.start_time.strftime('%Y-%m-%d %H:%M:%S')}")
        if self.dry_run:
            print("⚠️  DRY-RUN MODE - No files will be modified")
        print()

    def print_phase_info(self, phase_name: str) -> None:
        """Print information about a phase.
        
        Args:
            phase_name: Phase identifier (e.g., 'Phase2E')
        """
        phases = {**self.PHASES, **self.LARGE_PHASES}
        if phase_name not in phases:
            return

        phase = phases[phase_name]
        print(f"\n📋 {phase_name} - {phase['description']}")
        print("-" * 70)
        print(f"Features: {len(phase['features'])}")
        print(f"Operations: {phase['operations']}")
        print(f"Features to process:")
        for feature in phase['features']:
            print(f"  • {feature}")
        print()

    def run_batch_extraction(self, features: List[str]) -> bool:
        """Run batch extraction for a list of features.
        
        Args:
            features: List of feature names to process
            
        Returns:
            True if successful, False otherwise
        """
        cmd = ["python3", "batch_extract_contracts.py"]

        if features:
            cmd.extend(["--features"] + features)

        if self.dry_run:
            cmd.append("--dry-run")

        if self.verbose:
            cmd.append("--verbose")

        try:
            if self.verbose:
                print(f"🔄 Executing: {' '.join(cmd)}")
            
            result = subprocess.run(
                cmd,
                cwd=str(self.base_path),
                capture_output=False,
                timeout=1800  # 30 minutes timeout
            )
            
            return result.returncode == 0
        except subprocess.TimeoutExpired:
            print("❌ Batch extraction timeout (> 30 minutes)")
            return False
        except Exception as e:
            print(f"❌ Error running batch extraction: {e}")
            return False

    def verify_build(self) -> bool:
        """Verify that the solution builds successfully.
        
        Returns:
            True if build is successful, False otherwise
        """
        if self.dry_run:
            print("⏭️  Skipping build verification (dry-run mode)")
            return True

        print("\n🔨 Verifying build...")
        print("-" * 70)

        try:
            result = subprocess.run(
                ["dotnet", "build", "FSH.Framework.slnx", "-c", "Debug"],
                cwd=str(self.base_path / "src"),
                capture_output=True,
                timeout=600  # 10 minutes timeout
            )

            if result.returncode == 0:
                print("✅ Build verification PASSED\n")
                return True
            else:
                print("❌ Build verification FAILED")
                if self.verbose and result.stderr:
                    print("\nErrors:")
                    print(result.stderr.decode('utf-8', errors='ignore'))
                return False
        except subprocess.TimeoutExpired:
            print("❌ Build verification timeout (> 10 minutes)")
            return False
        except Exception as e:
            print(f"❌ Error verifying build: {e}")
            return False

    def run_phase(self, phase_name: str) -> Tuple[bool, int]:
        """Run extraction for a specific phase.
        
        Args:
            phase_name: Phase identifier (e.g., 'Phase2E')
            
        Returns:
            Tuple of (success, operations_count)
        """
        phases = {**self.PHASES, **self.LARGE_PHASES}
        
        if phase_name not in phases:
            print(f"❌ Unknown phase: {phase_name}")
            return False, 0

        phase = phases[phase_name]
        self.print_phase_info(phase_name)

        print(f"⏱️  Phase start: {datetime.now().strftime('%H:%M:%S')}")

        success = self.run_batch_extraction(phase["features"])

        if success:
            self.results[phase_name] = {
                "status": "✅ PASSED",
                "operations": phase["operations"],
                "time": datetime.now(),
            }
            print(f"\n✅ {phase_name} extraction COMPLETE ({phase['operations']} ops)\n")
        else:
            self.results[phase_name] = {
                "status": "❌ FAILED",
                "operations": 0,
                "time": datetime.now(),
            }
            print(f"\n❌ {phase_name} extraction FAILED\n")

        return success, phase["operations"] if success else 0

    def run_all_phases(self) -> bool:
        """Run extraction for all phases sequentially.
        
        Returns:
            True if all phases succeed, False otherwise
        """
        all_success = True
        total_ops = 0

        for phase_name in self.PHASES.keys():
            success, ops = self.run_phase(phase_name)
            
            if not success:
                all_success = False
                # Ask user if they want to continue
                if not self.dry_run:
                    response = input(
                        f"\n⚠️  {phase_name} failed. Continue with next phase? (y/n): "
                    )
                    if response.lower() != 'y':
                        break
            else:
                total_ops += ops

        # Build verification
        if all_success and not self.dry_run:
            if not self.verify_build():
                all_success = False

        return all_success

    def print_summary(self) -> None:
        """Print execution summary report."""
        elapsed = datetime.now() - self.start_time
        minutes = elapsed.total_seconds() / 60
        seconds = elapsed.total_seconds() % 60

        print("\n" + "=" * 70)
        print("📊 EXECUTION SUMMARY")
        print("=" * 70)

        if not self.results:
            print("No phases executed")
        else:
            total_ops = 0
            successful = 0
            
            for phase_name, result in self.results.items():
                status = result["status"]
                ops = result["operations"]
                total_ops += ops
                
                if "✅" in status:
                    successful += 1
                
                print(f"{status} {phase_name:12} ({ops:3} operations)")

            print(f"\n{'Total Operations':30}: {total_ops}")
            print(f"{'Time Elapsed':30}: {int(minutes):02d}m {int(seconds):02d}s")

            if successful == len(self.results):
                print(f"\n🎉 All phases completed successfully!")
            else:
                print(f"\n⚠️  Some phases failed - review output above")

        if self.dry_run:
            print("\n⚠️  This was a DRY-RUN - no files were modified")

        print("=" * 70 + "\n")

    def print_next_steps(self) -> None:
        """Print recommended next steps."""
        print("📋 NEXT STEPS:")
        print("-" * 70)
        
        if self.dry_run:
            print("1. Review the dry-run output above")
            print("2. Execute without --dry-run to apply changes")
            print("3. Commit changes to git")
            print("4. Continue with next phase if needed")
        else:
            print("1. ✅ Verify the build passed above")
            print("2. Run tests to validate extractions")
            print("3. Commit changes: git add . && git commit -m 'Phase 2E-Z contracts'")
            print("4. Continue with next phase: python3 run_phase_2ez.py --phase Phase2F")

        print()

    def main(self, phase_name: Optional[str] = None, run_all: bool = False) -> int:
        """Main orchestration logic.
        
        Args:
            phase_name: Specific phase to run (e.g., 'Phase2E')
            run_all: Run all phases
            
        Returns:
            Exit code (0 for success, 1 for failure)
        """
        self.print_header()

        # Verify we're in the correct directory
        if not (self.base_path / "src" / "Modules" / "Accounting").exists():
            print("❌ Error: Not in correct project directory")
            print(f"Expected: {self.base_path / 'src' / 'Modules' / 'Accounting'}")
            return 1

        # Execute based on arguments
        success = True

        if run_all:
            print("🔄 Running ALL phases (2E-Z)\n")
            success = self.run_all_phases()
        elif phase_name:
            success, _ = self.run_phase(phase_name)
        else:
            # Default: run Phase 2E
            print("🔄 Running Phase 2E (default)\n")
            success, _ = self.run_phase("Phase2E")

        # Print summary and next steps
        self.print_summary()
        
        if success:
            self.print_next_steps()

        return 0 if success else 1


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="Phase 2E-Z Automation Master Script",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  python3 run_phase_2ez.py --phase Phase2E
  python3 run_phase_2ez.py --all
  python3 run_phase_2ez.py --phase Phase2E --dry-run
  python3 run_phase_2ez.py --all --verbose
        """,
    )

    parser.add_argument(
        "--phase",
        choices=["Phase2E", "Phase2F", "Phase2G"],
        help="Run a specific phase",
    )
    parser.add_argument(
        "--all",
        action="store_true",
        help="Run all available phases (2E, 2F, 2G)",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Preview changes without modifying files",
    )
    parser.add_argument(
        "--verbose",
        action="store_true",
        help="Show detailed processing output",
    )

    args = parser.parse_args()

    # Determine what to run
    if not args.phase and not args.all:
        args.phase = "Phase2E"  # Default to Phase 2E

    # Create orchestrator and run
    orchestrator = Phase2EZOrchestrator(dry_run=args.dry_run, verbose=args.verbose)

    return orchestrator.main(phase_name=args.phase, run_all=args.all)


if __name__ == "__main__":
    sys.exit(main())
