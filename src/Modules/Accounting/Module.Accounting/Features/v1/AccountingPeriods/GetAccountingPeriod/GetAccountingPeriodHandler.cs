using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.AccountingPeriods;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.GetAccountingPeriod;

public record GetAccountingPeriodQuery(Guid Id) : IQuery<AccountingPeriodDto>;

public class GetAccountingPeriodHandler(AccountingDbContext context) : IQueryHandler<GetAccountingPeriodQuery, AccountingPeriodDto>
{
    public async ValueTask<AccountingPeriodDto> Handle(GetAccountingPeriodQuery query, CancellationToken ct)
    {
        var entity = await context.AccountingPeriods
            .Where(x => x.Id == query.Id)
            .Select(x => new AccountingPeriodDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AccountingPeriod not found");
    }
}
