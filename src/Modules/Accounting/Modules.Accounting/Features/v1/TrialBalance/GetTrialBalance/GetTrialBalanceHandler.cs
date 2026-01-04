using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.TrialBalance;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.GetTrialBalance;

public record GetTrialBalanceQuery(Guid Id) : IQuery<TrialBalanceDto>;

public class GetTrialBalanceHandler(AccountingDbContext context) : IQueryHandler<GetTrialBalanceQuery, TrialBalanceDto>
{
    public async ValueTask<TrialBalanceDto> Handle(GetTrialBalanceQuery query, CancellationToken ct)
    {
        var entity = await context.TrialBalance
            .Where(x => x.Id == query.Id)
            .Select(x => new TrialBalanceDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("TrialBalance not found");
    }
}
