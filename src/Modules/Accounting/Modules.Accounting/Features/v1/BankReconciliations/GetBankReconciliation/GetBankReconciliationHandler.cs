using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.BankReconciliations;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.GetBankReconciliation;

public record GetBankReconciliationQuery(Guid Id) : IQuery<BankReconciliationDto>;

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
