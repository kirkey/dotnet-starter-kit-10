# Quick Reference: Contract Extraction Automation

## 📁 Files Created
```
/extract_contracts.py              # Single feature extractor
/batch_extract_contracts.py        # Multi-feature batch processor
/CONTRACT_EXTRACTION_AUTOMATION.md # Full documentation
/AUTOMATION_READY.md               # Quick start guide
```

## 🚀 Quick Commands

### Test Mode (Safe - Preview Only)
```bash
# Preview what Phase 2E would extract
python3 extract_contracts.py --feature AccountingPeriods --dry-run

# Preview entire Phase 2E batch
python3 batch_extract_contracts.py --batch Phase2E --dry-run
```

### Execute Commands

#### Single Feature
```bash
# Extract one feature
python3 extract_contracts.py --feature AccountingPeriods

# With verbose output
python3 extract_contracts.py --feature AccountingPeriods --verbose
```

#### Phase Batches
```bash
# Phase 2E: 4 features (31 ops) → 100/340 total
python3 batch_extract_contracts.py --batch Phase2E

# Phase 2F: 4 features (25 ops) → 125/340 total
python3 batch_extract_contracts.py --batch Phase2F

# Phase 2G: 4 features (22 ops) → 147/340 total
python3 batch_extract_contracts.py --batch Phase2G
```

#### All Remaining Features
```bash
# Extract all Phase 2H-Z features (~193 ops) → 340+/340 total
python3 batch_extract_contracts.py --all
```

#### Custom Feature List
```bash
# Process specific features
python3 batch_extract_contracts.py --features \
  AccountingPeriods \
  BankReconciliations \
  DebitMemos
```

### Verify Build
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src
dotnet build FSH.Framework.slnx -c Debug
cd ..
```

## 📊 Progress Tracking

### Current Status
- **Completed**: 69/340 operations (20.3%)
  - Phase 1: 38 ops (11.2%)
  - Phase 2A-D: 31 ops (9.1%)

- **Remaining**: 271/340 operations (79.7%)
  - Phase 2E: 31 ops (9.1%)
  - Phase 2F: 25 ops (7.4%)
  - Phase 2G: 22 ops (6.5%)
  - Phase 2H-Z: 193 ops (56.8%)

### After Each Batch
```
After Phase 2E: 100/340 (29.4%)
After Phase 2F: 125/340 (36.8%)
After Phase 2G: 147/340 (43.2%)
After Phase 2H-Z: 340+/340 (100%)
```

## ⏱️ Expected Execution Times

| Command | Time | Operations |
|---------|------|------------|
| Phase 2E batch | 1-2 min | +31 ops |
| Phase 2F batch | 1-2 min | +25 ops |
| Phase 2G batch | 1-2 min | +22 ops |
| All remaining | 5-10 min | +193 ops |
| Build verify | 45-60 sec | - |
| **Total (all)** | **~15 min** | **+271 ops** |

## 🔍 Script Features

### extract_contracts.py
- ✅ Single feature processing
- ✅ Automatic handler detection
- ✅ Command/query extraction
- ✅ Contract file creation
- ✅ Handler/validator updating
- ✅ Code formatting
- ✅ Dry-run mode
- ✅ Verbose output

### batch_extract_contracts.py
- ✅ Multi-feature sequential processing
- ✅ Pre-defined phase groupings
- ✅ Timestamped progress
- ✅ Build verification
- ✅ Success/failure tracking
- ✅ Comprehensive reporting
- ✅ Custom feature lists
- ✅ Dry-run mode

## 🎯 Recommended Workflow

### Option 1: Gradual (Safest)
```bash
# 1. Test single feature
python3 extract_contracts.py --feature AccountingPeriods --dry-run
python3 extract_contracts.py --feature AccountingPeriods

# 2. Verify build
cd src && dotnet build FSH.Framework.slnx -c Debug && cd ..

# 3. Run phase batches
python3 batch_extract_contracts.py --batch Phase2E
python3 batch_extract_contracts.py --batch Phase2F
python3 batch_extract_contracts.py --batch Phase2G

# 4. Process remaining
python3 batch_extract_contracts.py --all
```

### Option 2: Fast (All at Once)
```bash
# Execute all remaining in one go
python3 batch_extract_contracts.py --all

# Verify
cd src && dotnet build FSH.Framework.slnx -c Debug
```

### Option 3: Custom Phases
```bash
# Define your own batches
python3 batch_extract_contracts.py --features \
  AccountingPeriods BankReconciliations DebitMemos CreditMemos \
  TaxRates TaxJournalEntries
```

## 📋 Phase Definitions

### Phase 2E (4 features)
```
AccountingPeriods      (7 ops)
BankReconciliations    (7 ops)
DebitMemos             (6 ops)
CreditMemos            (6 ops)
───────────────────────────────
Total: 31 operations
```

### Phase 2F (4 features)
```
TaxRates               (6 ops)
TaxJournalEntries      (7 ops)
ExpenseCategories      (6 ops)
RevenueCategories      (6 ops)
───────────────────────────────
Total: 25 operations
```

### Phase 2G (4 features)
```
SubLedgers             (6 ops)
GeneralLedgerAccounts  (7 ops)
AccountMappings        (5 ops)
DimensionValues        (4 ops)
───────────────────────────────
Total: 22 operations
```

### Phase 2H-Z (100+ features)
```
Multiple features (193+ operations)
Including:
- GLNarrative, GLNotes, GLAttachments
- TransactionReversal, TransactionApproval
- AuditLog, ComplianceReview
- BudgetAnalysis, ForecastAnalysis
- And 80+ more features
───────────────────────────────
Total: 193+ operations
```

## 🛡️ Safety Checklist

Before running each batch:
- [ ] Current directory: `/Users/kirkeypsalms/Projects/dotnet-starter-kit-10`
- [ ] Python 3.7+ installed: `python3 --version`
- [ ] .NET SDK available: `dotnet --version`
- [ ] Scripts executable: `ls -la extract_contracts.py`
- [ ] Last build passes: `cd src && dotnet build FSH.Framework.slnx -c Debug`

## 🔧 Troubleshooting

### Feature Not Found
```bash
# Verify feature directory exists
ls -la src/Modules/Accounting/Module.Accounting/Features/v1/AccountingPeriods
```

### No Operations Extracted
```bash
# Check handler structure
ls -la src/Modules/Accounting/Module.Accounting/Features/v1/AccountingPeriods/*/
```

### Build Errors After Extraction
```bash
# Verify using statements are correct
grep -r "using FSH.Module.Accounting.Contracts" src/Modules/Accounting/

# Format code
cd src && dotnet format . && cd ..
```

### Python Script Issues
```bash
# Check Python version
python3 --version  # Should be 3.7+

# Check script syntax
python3 -m py_compile extract_contracts.py

# Run with explicit Python path
/usr/bin/python3 extract_contracts.py --feature AccountingPeriods
```

## 📈 Progress Reporting

After each batch completes, update:
```markdown
Phase 2E: ✅ COMPLETE (100/340 = 29.4%)
Phase 2F: ⏳ IN PROGRESS
Phase 2G: ⏳ PENDING
Phase 2H-Z: ⏳ PENDING
```

## 🎓 Documentation

| Document | Purpose |
|----------|---------|
| CONTRACT_EXTRACTION_AUTOMATION.md | Complete technical reference |
| AUTOMATION_READY.md | Quick start guide |
| QUICK_REFERENCE.md | This document |

## 📞 Support

For detailed help, see:
- `CONTRACT_EXTRACTION_AUTOMATION.md` - Full documentation
- `AUTOMATION_READY.md` - Workflow recommendations
- Script help: `python3 extract_contracts.py --help`

---

**Last Updated**: January 4, 2026
**Status**: ✅ Ready for execution
**Version**: 1.0
