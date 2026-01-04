using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Accruals;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Accruals.GetAccrual;

public record GetAccrualQuery(Guid Id) : IQuery<AccrualDto>;

public class GetAccrualHandler(AccountingDbContext context) : IQueryHandler<GetAccrualQuery, AccrualDto>
{
    public async ValueTask<AccrualDto> Handle(GetAccrualQuery query, CancellationToken ct)
    {
        var entity = await context.Accruals
            .Where(x => x.Id == query.Id)
            .Select(x => new AccrualDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Accrual not found");
    }
}
