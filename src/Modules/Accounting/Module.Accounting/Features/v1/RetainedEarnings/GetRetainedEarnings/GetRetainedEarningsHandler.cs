using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;

public record GetRetainedEarningsQuery(Guid Id) : IQuery<RetainedEarningsDto>;

public class GetRetainedEarningsHandler(AccountingDbContext context) : IQueryHandler<GetRetainedEarningsQuery, RetainedEarningsDto>
{
    public async ValueTask<RetainedEarningsDto> Handle(GetRetainedEarningsQuery query, CancellationToken ct)
    {
        var entity = await context.RetainedEarnings
            .Where(x => x.Id == query.Id)
            .Select(x => new RetainedEarningsDto(
                x.Id,
                x.Name,
                x.Description,
                x.FiscalYear,
                x.OpeningBalance,
                x.ClosingBalance,
                x.IsClosed,
                x.ClosedOn,
                x.ClosedBy,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RetainedEarnings not found");
    }
}
