# Accounting Module Contract Extraction - Implementation Guide

**Status**: Phase 1 Priority 1A - ChartOfAccounts Complete ✅  
**Next**: JournalEntries, Invoices, Bills (16 operations total)  
**Estimated Remaining**: 20-30 hours using this systematic approach  

---

## What Was Completed (ChartOfAccounts - 5 operations)

### ✅ Contract Files Created
```
Module.Accounting.Contracts/v1/ChartOfAccounts/
├── CreateChartOfAccount/
│   └── CreateChartOfAccountCommand.cs            ✅ NEW
├── UpdateChartOfAccount/
│   └── UpdateChartOfAccountCommand.cs            ✅ NEW
├── DeleteChartOfAccount/
│   └── DeleteChartOfAccountCommand.cs            ✅ NEW
├── GetChartOfAccount/
│   └── GetChartOfAccountQuery.cs                 ✅ NEW
├── GetChartOfAccounts/
│   └── GetChartOfAccountsQuery.cs                ✅ NEW
│       (includes ChartOfAccountsPagedResponse)
└── ChartOfAccountDto.cs                          (existing)
```

### ✅ Handler Files Updated
- CreateChartOfAccountHandler.cs - Added Contracts using reference
- UpdateChartOfAccountHandler.cs - Added Contracts using reference
- DeleteChartOfAccountHandler.cs - Added Contracts using reference
- GetChartOfAccountHandler.cs - Added Contracts using reference
- GetChartOfAccountsHandler.cs - Added Contracts using reference

### ✅ Validator Files Updated
- CreateChartOfAccountValidator.cs - Added Contracts using reference
- UpdateChartOfAccountValidator.cs - Added Contracts using reference

---

## How to Repeat This Pattern (For JournalEntries, Invoices, Bills, etc.)

### Template: Extract Command from Handler

**Step 1: Read the Handler File**
```bash
# Example for CreateJournalEntry
cat src/Modules/Accounting/Module.Accounting/Features/v1/JournalEntries/CreateJournalEntry/CreateJournalEntryHandler.cs | head -50
```

Look for the line containing `public record {Feature}Command(...)  : ICommand<...>`

**Step 2: Copy the entire record definition** (including all XML documentation above it)

**Step 3: Create the Contract File**
```bash
# Example for CreateJournalEntry
touch src/Modules/Accounting/Module.Accounting.Contracts/v1/JournalEntries/CreateJournalEntry/CreateJournalEntryCommand.cs
```

**Step 4: Paste into the contract file with proper namespace**
```csharp
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.CreateJournalEntry;

// PASTE THE RECORD HERE (e.g., public record CreateJournalEntryCommand(...) : ICommand<Guid>;)
```

**Step 5: Update the handler file**
- Add using: `using FSH.Module.Accounting.Contracts.v1.JournalEntries.CreateJournalEntry;`
- Remove the inline command definition (keep only the handler class)
- Keep all handler XML documentation

**Step 6: Update the validator file** (if it exists)
- Add using: `using FSH.Module.Accounting.Contracts.v1.JournalEntries.CreateJournalEntry;`
- Ensure it references the imported command

**Step 7: Update the endpoint file** (if it references the command)
- Add using: `using FSH.Module.Accounting.Contracts.v1.JournalEntries.CreateJournalEntry;`

---

## Batch Extraction: JournalEntries (8 operations)

### Operations to Extract:
1. **CreateJournalEntry** → CreateJournalEntryCommand.cs
2. **UpdateJournalEntry** → UpdateJournalEntryCommand.cs
3. **DeleteJournalEntry** → DeleteJournalEntryCommand.cs
4. **PostJournalEntry** → PostJournalEntryCommand.cs
5. **ApproveJournalEntry** → ApproveJournalEntryCommand.cs
6. **ReverseJournalEntry** → ReverseJournalEntryCommand.cs
7. **GetJournalEntry** → GetJournalEntryQuery.cs
8. **GetListJournalEntry** → GetJournalEntryListQuery.cs (with response DTOs)

### Estimated Effort per Operation:
- Read handler: 2 minutes
- Extract command: 3 minutes
- Create contract file: 2 minutes
- Update handler using: 2 minutes
- Update validator using: 1 minute
- Format & verify: 2 minutes
- **Total per operation: ~12 minutes**
- **For 8 operations: ~96 minutes (~2 hours)**

### Implementation Checklist:
```
CreateJournalEntry:
  [ ] Extract CreateJournalEntryCommand to Contracts
  [ ] Update CreateJournalEntryHandler imports
  [ ] Update CreateJournalEntryValidator imports
  [ ] Verify no compile errors

UpdateJournalEntry:
  [ ] Extract UpdateJournalEntryCommand to Contracts
  [ ] Update UpdateJournalEntryHandler imports
  [ ] Update UpdateJournalEntryValidator imports
  [ ] Verify no compile errors

DeleteJournalEntry:
  [ ] Extract DeleteJournalEntryCommand to Contracts
  [ ] Update DeleteJournalEntryHandler imports
  [ ] Verify no compile errors

PostJournalEntry:
  [ ] Extract PostJournalEntryCommand to Contracts
  [ ] Update PostJournalEntryHandler imports
  [ ] Update PostJournalEntryValidator imports
  [ ] Verify no compile errors

ApproveJournalEntry:
  [ ] Extract ApproveJournalEntryCommand to Contracts
  [ ] Update ApproveJournalEntryHandler imports
  [ ] Update ApproveJournalEntryValidator imports
  [ ] Verify no compile errors

ReverseJournalEntry:
  [ ] Extract ReverseJournalEntryCommand to Contracts
  [ ] Update ReverseJournalEntryHandler imports
  [ ] Update ReverseJournalEntryValidator imports
  [ ] Verify no compile errors

GetJournalEntry:
  [ ] Extract GetJournalEntryQuery to Contracts
  [ ] Update GetJournalEntryHandler imports
  [ ] Verify no compile errors

GetListJournalEntry:
  [ ] Extract GetJournalEntryListQuery to Contracts
  [ ] Extract response DTOs (if defined in handler)
  [ ] Update GetJournalEntryListHandler imports
  [ ] Verify no compile errors
```

---

## Batch Extraction: Invoices (7 operations)

### Operations to Extract:
1. CreateInvoice → CreateInvoiceCommand.cs
2. UpdateInvoice → UpdateInvoiceCommand.cs
3. DeleteInvoice → DeleteInvoiceCommand.cs
4. ApproveInvoice → ApproveInvoiceCommand.cs
5. SendInvoice → SendInvoiceCommand.cs
6. GetInvoice → GetInvoiceQuery.cs
7. GetListInvoice → GetInvoiceListQuery.cs

**Estimated Effort**: 7 × 12 minutes = **~1.4 hours**

---

## Batch Extraction: Bills (6 operations)

### Operations to Extract:
1. CreateBill → CreateBillCommand.cs
2. UpdateBill → UpdateBillCommand.cs
3. DeleteBill → DeleteBillCommand.cs
4. ApproveBill → ApproveBillCommand.cs
5. GetBill → GetBillQuery.cs
6. GetListBill → GetBillListQuery.cs

**Estimated Effort**: 6 × 12 minutes = **~1.2 hours**

---

## Special Cases Handling

### Queries with Response DTOs

Some queries define custom response DTOs inline in the handler (e.g., `GetChartOfAccountsQuery` returns `ChartOfAccountsPagedResponse`).

**Rule**: Extract response DTOs to the same Contracts file as the query

Example (from GetChartOfAccounts):
```csharp
// File: GetChartOfAccountsQuery.cs (Contracts project)
using Mediator;

public record GetChartOfAccountsQuery(...) : IQuery<ChartOfAccountsPagedResponse>;

public record ChartOfAccountsPagedResponse(
    List<ChartOfAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public record ChartOfAccountSummaryDto(...);
```

### Import Statements in Contracts

If a Command/Query references DTOs from Contracts (e.g., `ChartOfAccountDto`), include that using:

```csharp
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.CreateChartOfAccount;

public record CreateChartOfAccountCommand(...) : ICommand<Guid>;
```

---

## Priority 1B: Finance Operations (17 operations)

### Payments (5 operations):
- CreatePayment → CreatePaymentCommand.cs
- UpdatePayment → UpdatePaymentCommand.cs
- DeletePayment → DeletePaymentCommand.cs
- ApprovePayment → ApprovePaymentCommand.cs
- GetPayment, GetListPayment → Queries

**Estimated**: 5 × 12 min = 1 hour

### PostingBatches (6 operations):
- CreatePostingBatch → CreatePostingBatchCommand.cs
- ApprovePostingBatch → ApprovePostingBatchCommand.cs
- PostPostingBatch → PostPostingBatchCommand.cs
- RejectPostingBatch → RejectPostingBatchCommand.cs
- GetPostingBatch, GetListPostingBatch → Queries

**Estimated**: 6 × 12 min = 1.2 hours

### Budgets (6 operations):
- CreateBudget → CreateBudgetCommand.cs
- UpdateBudget → UpdateBudgetCommand.cs
- DeleteBudget → DeleteBudgetCommand.cs
- ApproveBudget → ApproveBudgetCommand.cs
- GetBudget, GetListBudget → Queries

**Estimated**: 6 × 12 min = 1.2 hours

**Total for Priority 1B**: ~3.4 hours

---

## Full Extraction Timeline

| Phase | Features | Operations | Estimated Time |
|-------|----------|-----------|-----------------|
| **Done** ✅ | ChartOfAccounts | 5 | 1 hour |
| **Phase 1A** | JournalEntries, Invoices, Bills | 21 | 3.6 hours |
| **Phase 1B** | Payments, PostingBatches, Budgets | 17 | 3.4 hours |
| **Phase 2** | Remaining ~100 features | ~300 operations | 40-50 hours |
| **Total** | ~150 features | ~340 operations | **50-60 hours** |

---

## Optimization Strategy

### For Manual Extraction (Phase 1):
Use this step-by-step guide. Est. 8 hours for Phase 1A+1B.

### For Phase 2 (100+ features):
Consider automating with a PowerShell or Python script that:
1. Scans each feature handler folder
2. Extracts record definitions using regex
3. Creates contract files
4. Updates using statements
5. Applies formatter

---

## Validation Commands

After each feature extraction:

```bash
# Check for compilation errors
dotnet build src/Modules/Accounting/

# Format the code
dotnet format src/Modules/Accounting/

# Run tests (if applicable)
dotnet test
```

---

## Next Steps

1. **Phase 1A (Immediate - 3.6 hours)**:
   - Extract JournalEntries (8 ops)
   - Extract Invoices (7 ops)
   - Extract Bills (6 ops)

2. **Phase 1B (Next - 3.4 hours)**:
   - Extract Payments (5 ops)
   - Extract PostingBatches (6 ops)
   - Extract Budgets (6 ops)

3. **Phase 2 (Longer term - 40-50 hours)**:
   - Automate extraction for remaining ~100 features
   - Or continue manual extraction in batches

4. **Final Validation**:
   - Full build verification
   - All imports correct
   - All tests passing
   - Code formatted consistently

---

## Files Affected Summary

By feature extraction:
- **ChartOfAccounts**: 7 files modified, 5 contracts created ✅
- **JournalEntries** (next): ~11 files to modify, 8 contracts to create
- **Invoices** (next): ~10 files to modify, 7 contracts to create
- **Bills** (next): ~9 files to modify, 6 contracts to create

**Total Phase 1A**: 37 files to update, 21 contracts to create

---

## Success Criteria

✅ All Commands/Queries in separate Contracts files  
✅ All handler/validator/endpoint imports reference Contracts  
✅ Zero compilation errors  
✅ Consistent with COPILOT_INSTRUCTIONS.md  
✅ Matches Todos module pattern  
✅ Code formatted properly  

---

## Support Resources

- **Reference Pattern**: src/Modules/Todos/Module.Todos.Contracts/v1/Todos/
- **Instructions**: COPILOT_INSTRUCTIONS.md (lines 85-120)
- **Audit Report**: ACCOUNTING_ALIGNMENT_AUDIT.md
- **This Guide**: ACCOUNTING_CONTRACT_EXTRACTION_GUIDE.md

