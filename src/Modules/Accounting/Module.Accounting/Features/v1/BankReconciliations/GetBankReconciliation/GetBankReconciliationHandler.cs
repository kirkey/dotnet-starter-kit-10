using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.GetBankReconciliation;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.GetBankReconciliation;


/// <summary>
/// Handler for retrieving a single BankReconciliation with DTO projection.
/// </summary>
/// <remarks>
/// Responsibility: Project the BankReconciliation aggregate to a DTO including balances, status, and reconciliation metadata.
/// 
/// Execution Flow:
/// 1. Query BankReconciliations DbSet by Id
/// 2. Project to BankReconciliationDto with reconciliation fields (Id, ReconciliationNumber, BankAccountId, StatementDate, StatementBalance, BookBalance, Difference, Status, ReconciledDate, ReconciledBy, AdjustmentAmount, AdjustmentNotes, Description, IsActive, CreatedOnUtc)
/// 3. Throw NotFoundException if not found
/// 
/// Use Cases: Review reconciliation details, audit, and troubleshooting differences between statement and books.
/// 
/// Permissions: Requires authenticated user
/// 
/// Exceptions:
/// - NotFoundException: Thrown if reconciliation not found
/// </remarks>
public class GetBankReconciliationHandler(AccountingDbContext context) : IQueryHandler<GetBankReconciliationQuery, BankReconciliationDto>
{
    public async ValueTask<BankReconciliationDto> Handle(GetBankReconciliationQuery query, CancellationToken ct)
    {
        var entity = await context.BankReconciliations
            .Where(x => x.Id == query.Id)
            .Select(x => new BankReconciliationDto(
                x.Id,
                x.ReconciliationNumber,
                x.BankAccountId,
                x.StatementDate,
                x.StatementBalance,
                x.BookBalance,
                x.Difference,
                x.Status,
                x.ReconciledDate,
                x.ReconciledBy,
                x.AdjustmentAmount,
                x.AdjustmentNotes,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("BankReconciliation not found");
    }
} 
