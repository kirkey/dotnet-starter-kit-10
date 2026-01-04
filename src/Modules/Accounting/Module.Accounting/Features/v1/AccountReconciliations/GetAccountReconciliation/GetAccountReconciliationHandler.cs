using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.AccountReconciliations;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.AccountReconciliations.GetAccountReconciliation;

public record GetAccountReconciliationQuery(Guid Id) : IQuery<AccountReconciliationDto>;

public class GetAccountReconciliationHandler(AccountingDbContext context) : IQueryHandler<GetAccountReconciliationQuery, AccountReconciliationDto>
{
    public async ValueTask<AccountReconciliationDto> Handle(GetAccountReconciliationQuery query, CancellationToken ct)
    {
        var entity = await context.AccountReconciliations
            .Where(x => x.Id == query.Id)
            .Select(x => new AccountReconciliationDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AccountReconciliation not found");
    }
}
