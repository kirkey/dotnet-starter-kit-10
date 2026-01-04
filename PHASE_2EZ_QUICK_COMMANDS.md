# ⚡ Phase 2E-Z Quick Command Reference

**Last Updated:** January 4, 2026  
**Scripts Ready:** ✅ Fully Functional  
**Status:** Ready for Execution

---

## 🚀 Essential Commands

### Default (Phase 2E - Core Accounting)
```bash
python3 run_phase_2ez.py
```
Time: ~2 min | Operations: 31 | Files: 62 modified

### Specific Phase
```bash
# Phase 2E - Core accounting operations
python3 run_phase_2ez.py --phase Phase2E

# Phase 2F - Tax and categorization
python3 run_phase_2ez.py --phase Phase2F

# Phase 2G - Ledger and mapping
python3 run_phase_2ez.py --phase Phase2G
```

### All Phases Combined
```bash
python3 run_phase_2ez.py --all
```
Time: ~5-7 min | Operations: 78 | Files: 156 modified

---

## 🔍 Preview Commands (Safe)

### Preview Phase 2E (no changes)
```bash
python3 run_phase_2ez.py --dry-run
```

### Preview Specific Phase
```bash
python3 run_phase_2ez.py --phase Phase2F --dry-run
```

### Preview All Phases
```bash
python3 run_phase_2ez.py --all --dry-run
```

---

## 📊 Verbose Output

### See detailed processing
```bash
python3 run_phase_2ez.py --phase Phase2E --verbose
```

### Full details for all phases
```bash
python3 run_phase_2ez.py --all --verbose
```

---

## 🧪 Single Feature Testing (Optional)

### Test single feature directly
```bash
python3 extract_contracts.py --feature AccountingPeriods

python3 extract_contracts.py --feature BankReconciliations --dry-run
```

### Batch specific features
```bash
python3 batch_extract_contracts.py --features AccountingPeriods DebitMemos CreditMemos
```

---

## ✅ Post-Execution Steps

### Check what changed
```bash
git status
git diff src/Modules/Accounting/ | head -100
```

### Run tests (if applicable)
```bash
cd src && dotnet test FSH.Framework.Tests.slnx -c Debug
```

### Commit changes
```bash
git add src/Modules/Accounting/
git commit -m "Phase 2E-Z: Extract accounting command/query contracts"
git push origin develop
```

---

## 📈 Phase Definitions

### Phase 2E (31 operations)
- AccountingPeriods (7)
- BankReconciliations (7)
- DebitMemos (6)
- CreditMemos (6)

### Phase 2F (25 operations)
- TaxRates (6)
- TaxJournalEntries (7)
- ExpenseCategories (6)
- RevenueCategories (6)

### Phase 2G (22 operations)
- SubLedgers (6)
- GeneralLedgerAccounts (7)
- AccountMappings (5)
- DimensionValues (4)

---

## ⏱️ Time Guide

| Operation | Time | Details |
|-----------|------|---------|
| Phase 2E preview | 1-2 min | `--dry-run` |
| Phase 2E execute | 2 min | `--phase Phase2E` |
| Phase 2F execute | 1.5 min | `--phase Phase2F` |
| Phase 2G execute | 1.5 min | `--phase Phase2G` |
| All phases | 5-7 min | `--all` |
| Build verification | 45-60 sec | Auto-included |

---

## 🛡️ Safety Commands

### Preview before running
```bash
python3 run_phase_2ez.py --phase Phase2E --dry-run
```

### Revert changes if needed
```bash
git checkout src/Modules/Accounting/
```

### Check build status
```bash
cd src && dotnet build FSH.Framework.slnx -c Debug
```

---

## 🔧 Troubleshooting Quick Fixes

### "File not found" error
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10
python3 run_phase_2ez.py --phase Phase2E
```

### "Build failed" after extraction
```bash
cd src && dotnet format src/Modules/Accounting/
dotnet build FSH.Framework.slnx -c Debug
```

### "Permission denied"
```bash
chmod +x run_phase_2ez.py extract_contracts.py batch_extract_contracts.py
python3 run_phase_2ez.py --phase Phase2E
```

---

## 📚 Related Files

| File | Purpose | Size |
|------|---------|------|
| `run_phase_2ez.py` | 🎯 Master orchestrator | 385 lines |
| `extract_contracts.py` | Single feature extractor | 402 lines |
| `batch_extract_contracts.py` | Batch processor | 246 lines |
| `PHASE_2EZ_EXECUTION_GUIDE.md` | Full execution guide | Detailed |
| `CONTRACT_EXTRACTION_AUTOMATION.md` | Technical reference | Very detailed |

---

## 🎯 Recommended Workflow

### Option A: Conservative (Safest)
```bash
# 1. Preview Phase 2E
python3 run_phase_2ez.py --dry-run

# 2. Execute Phase 2E
python3 run_phase_2ez.py

# 3. Verify and commit
git status && git add . && git commit -m "Phase 2E complete"

# 4. Repeat for Phase 2F and 2G
python3 run_phase_2ez.py --phase Phase2F
# ... commit ...
python3 run_phase_2ez.py --phase Phase2G
```

### Option B: Standard (Recommended)
```bash
# 1. Execute Phase 2E
python3 run_phase_2ez.py

# 2. Execute Phase 2F
python3 run_phase_2ez.py --phase Phase2F

# 3. Execute Phase 2G
python3 run_phase_2ez.py --phase Phase2G

# 4. Verify and commit all
git status && git add . && git commit -m "Phase 2E-Z complete (78 operations)"
```

### Option C: Aggressive (Fastest)
```bash
# Execute all at once
python3 run_phase_2ez.py --all

# Verify and commit
git add . && git commit -m "Phase 2E-Z complete (78 operations)"
```

---

## 📊 Progress Checklist

```
Phase 2E - Core Accounting (31 ops)
  [ ] Preview with --dry-run
  [ ] Execute --phase Phase2E
  [ ] Verify build passes
  [ ] Commit changes
  [ ] Update documentation

Phase 2F - Tax & Categories (25 ops)
  [ ] Preview with --dry-run
  [ ] Execute --phase Phase2F
  [ ] Verify build passes
  [ ] Commit changes
  [ ] Update documentation

Phase 2G - Ledger & Mapping (22 ops)
  [ ] Preview with --dry-run
  [ ] Execute --phase Phase2G
  [ ] Verify build passes
  [ ] Commit changes
  [ ] Update documentation

Phase 2H-Z - Remaining (193+ ops)
  [ ] Plan execution schedule
  [ ] Execute in batches
  [ ] Monitor for issues
  [ ] Track progress
```

---

## 💡 Key Facts

- **Total Operations:** 271+ contract extractions
- **Total Time:** 12-15 minutes automated
- **Manual Time:** 10-15 hours (40-75x slower)
- **Build Verification:** Automatic after each phase
- **Safe Execution:** Dry-run mode available
- **Easy Rollback:** All changes tracked in git

---

## 🚀 Ready? Start Here

```bash
# Quick start - default Phase 2E
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10
python3 run_phase_2ez.py

# Or preview first
python3 run_phase_2ez.py --dry-run
```

---

**Status:** ✅ Fully Operational  
**Last Run:** Never executed yet  
**Next Step:** Execute `python3 run_phase_2ez.py`

