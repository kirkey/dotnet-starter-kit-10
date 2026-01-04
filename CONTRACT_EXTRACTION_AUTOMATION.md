# Contract Extraction Automation Scripts

## Overview

These Python scripts automate the extraction of embedded command/query definitions from Accounting module handlers to separate contract files, enabling the Phase 2E-Z operations (~255 remaining operations).

## Scripts

### 1. `extract_contracts.py` - Single Feature Extractor

Extracts commands/queries from a single feature's handlers and creates contract files.

**Usage:**
```bash
python3 extract_contracts.py --feature FeatureName [options]
```

**Options:**
- `--feature NAME` (required): Feature to process (e.g., `AccountingPeriods`)
- `--dry-run`: Preview changes without modifying files
- `--verbose`: Show detailed processing output
- `--no-format`: Skip running dotnet format after extraction

**Examples:**
```bash
# Process single feature
python3 extract_contracts.py --feature AccountingPeriods

# Preview changes without modifying
python3 extract_contracts.py --feature BankReconciliations --dry-run

# Verbose output for troubleshooting
python3 extract_contracts.py --feature DebitMemos --verbose
```

**What it does:**
1. Scans all `*Handler.cs` files in the feature directory
2. Extracts embedded `public record` command/query definitions
3. Creates contract files in `Contracts/v1/{FeatureName}/{Operation}/`
4. Updates handlers to reference contracts instead of embedded definitions
5. Updates validators to use contract using statements
6. Runs dotnet format on the Accounting module
7. Reports operations extracted and files modified

**Output example:**
```
🔄 Processing feature: AccountingPeriods
============================================================

📋 Operation: CreateAccountingPeriod
  ✅ Created: CreateAccountingPeriodCommand.cs
  ✅ Updated handler: CreateAccountingPeriodHandler.cs
  ✅ Updated validator: CreateAccountingPeriodValidator.cs

[... more operations ...]

📊 EXTRACTION REPORT
============================================================
Feature: AccountingPeriods
Operations extracted: 7
Files created: 7
Files updated: 14
```

---

### 2. `batch_extract_contracts.py` - Multi-Feature Batch Processor

Processes multiple features sequentially with build verification.

**Usage:**
```bash
python3 batch_extract_contracts.py [--features F1 F2 F3 | --batch PHASE | --all] [options]
```

**Options:**
- `--features F1 F2 F3`: Specific features to process
- `--batch PHASE`: Pre-defined phase batch
  - `Phase2E`: AccountingPeriods, BankReconciliations, DebitMemos, CreditMemos
  - `Phase2F`: TaxRates, TaxJournalEntries, ExpenseCategories, RevenueCategories
  - `Phase2G`: SubLedgers, GeneralLedgerAccounts, AccountMappings, DimensionValues
- `--all`: Process all Phase 2E-Z features (~100 features)
- `--dry-run`: Preview without making changes
- `--verbose`: Show detailed output

**Examples:**
```bash
# Process Phase 2E batch
python3 batch_extract_contracts.py --batch Phase2E

# Process multiple specific features
python3 batch_extract_contracts.py --features AccountingPeriods BankReconciliations DebitMemos

# Preview entire Phase 2E without changes
python3 batch_extract_contracts.py --batch Phase2E --dry-run

# Process all features with verbose output
python3 batch_extract_contracts.py --all --verbose
```

**Features:**
- Processes features sequentially to maintain order
- Tracks success/failure for each feature
- Verifies build after all extractions complete
- Provides timestamp-based progress logging
- Comprehensive summary report

**Output example:**
```
[12:34:56] 🚀 BATCH CONTRACT EXTRACTION START
[12:34:56] Features to process: 4
[12:34:56] Dry-run: False

[12:35:01] [1/4] Processing AccountingPeriods...
[12:35:05] ✅ AccountingPeriods - COMPLETE
[12:35:06] [2/4] Processing BankReconciliations...
[12:35:12] ✅ BankReconciliations - COMPLETE
[...]

[12:36:45] 🔨 Verifying build...
[12:37:30] ✅ Build verification PASSED

📊 BATCH EXTRACTION SUMMARY
============================================================
Total features: 4
Successful: 4
Failed: 0
Time elapsed: 94.2s
============================================================
```

---

## Phase Organization

### Phase 2E (4 features, ~31 operations)
- AccountingPeriods (7)
- BankReconciliations (7)
- DebitMemos (6)
- CreditMemos (6)

### Phase 2F (4 features, ~25 operations)
- TaxRates (6)
- TaxJournalEntries (7)
- ExpenseCategories (6)
- RevenueCategories (6)

### Phase 2G (4 features, ~22 operations)
- SubLedgers (6)
- GeneralLedgerAccounts (7)
- AccountMappings (5)
- DimensionValues (4)

*Note: Remaining phases (2H-Z) contain ~220+ additional operations across 80+ features*

---

## Recommended Workflow

### 1. Single Feature Testing (Dry-run)
```bash
# Preview what will change
python3 extract_contracts.py --feature AccountingPeriods --dry-run
```

### 2. Single Feature Execution
```bash
# Process one feature to verify pattern works
python3 extract_contracts.py --feature AccountingPeriods
```

### 3. Phase Batch Processing
```bash
# Process entire Phase 2E
python3 batch_extract_contracts.py --batch Phase2E

# Or process with dry-run first
python3 batch_extract_contracts.py --batch Phase2E --dry-run
```

### 4. Build Verification
```bash
# After batch processing, verify build
cd src && dotnet build FSH.Framework.slnx -c Debug
```

### 5. Progress Tracking
Update the progress file after each batch:
- Phase 2E: +31 operations
- Phase 2F: +25 operations  
- Phase 2G: +22 operations

---

## How It Works

### Extraction Pattern

1. **Find Handlers**: Locates `*Handler.cs` files in feature directory
2. **Extract Definition**: Uses regex to find `public record CommandName(...) : ICommand<T>;`
3. **Extract DTOs**: For GetList queries, also extracts `PagedResponse` and `SummaryDto` records
4. **Extract XML Docs**: Preserves XML documentation comments
5. **Create Contract File**: Writes to `Contracts/v1/{Feature}/{Operation}/CommandName.cs`
6. **Update Handler**: Removes embedded definition, adds using statement for contract
7. **Update Validator**: Adds using statement for contract (if validator exists)
8. **Format Code**: Runs `dotnet format` for consistency

### File Operations

**Created:** Contract files in proper namespace structure
```
Contracts/v1/FeatureName/OperationName/CommandName.cs
```

**Updated:** Handler and validator files with new using statements
```
Features/v1/FeatureName/OperationName/CommandNameHandler.cs
Features/v1/FeatureName/OperationName/CommandNameValidator.cs
```

---

## Safety Features

### Dry-run Mode
Always preview before executing:
```bash
python3 extract_contracts.py --feature NewFeature --dry-run
```

### No File Deletion
Scripts only create and update files - nothing is deleted.

### Build Verification
Batch processor automatically verifies build after extractions.

### Atomic Transactions
Each feature is independent - failure in one doesn't affect others.

---

## Troubleshooting

### Feature not found
```
❌ No handlers found for FeatureName
```
**Solution:** Verify feature directory exists in `src/Modules/Accounting/Module.Accounting/Features/v1/`

### No operations extracted
```
⚠️  No command/query found
```
**Solution:** Check handler files for proper `public record` definition with `ICommand<T>` or `IQuery<T>`

### Build errors after extraction
```
❌ Build verification FAILED
```
**Solution:** 
1. Check for missing using statements
2. Verify namespace format: `FSH.Module.Accounting.Contracts.v1.{Feature}.{Operation}`
3. Run dotnet format: `dotnet format src/Modules/Accounting/`

### Permission denied
```
PermissionError: [Errno 13] Permission denied
```
**Solution:** Make scripts executable:
```bash
chmod +x extract_contracts.py batch_extract_contracts.py
```

---

## Performance Metrics

Estimated extraction times (based on Phase 1 and 2A-D):

| Operation | Time |
|-----------|------|
| Single feature extraction | 2-5 seconds |
| Phase batch (3-4 features) | 15-30 seconds |
| Build verification | 45-60 seconds |
| Full Phase 2E-Z (~100 features) | 8-12 minutes |

---

## Progress Tracking

After each batch execution, update the todo list:

```bash
# After Phase 2E
python3 batch_extract_contracts.py --batch Phase2E
# +31 operations = 100/340 (29.4%)

# After Phase 2F  
python3 batch_extract_contracts.py --batch Phase2F
# +25 operations = 125/340 (36.8%)

# Continue with remaining phases...
```

---

## Integration with CI/CD

These scripts can be integrated into build pipelines:

```yaml
# Example GitHub Actions
- name: Extract contracts for Phase 2
  run: |
    python3 batch_extract_contracts.py --batch Phase2E
    
- name: Verify build
  run: |
    cd src && dotnet build FSH.Framework.slnx -c Debug
```

---

## Requirements

- Python 3.7+
- .NET 10.0 SDK
- `dotnet` CLI accessible from PATH
- Write permissions on project directory

## Next Steps

1. ✅ Review Phase 2E feature list
2. 🔄 Run: `python3 batch_extract_contracts.py --batch Phase2E --dry-run`
3. ✅ Verify preview looks correct
4. 🚀 Execute: `python3 batch_extract_contracts.py --batch Phase2E`
5. ✅ Verify build passes
6. 📊 Update progress (100/340 ops = 29.4%)
7. 🔄 Repeat for Phase 2F, 2G, etc.

---

**Created:** January 4, 2026  
**Status:** Ready for Phase 2E-Z extraction  
**Expected completion:** ~12-15 hours for all remaining operations
