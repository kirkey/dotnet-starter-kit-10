using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.PromiseToPays;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.GetPromiseToPay;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.GetPromiseToPay;

public class GetPromiseToPayHandler(MicrofinanceDbContext context) : IQueryHandler<GetPromiseToPayQuery, PromiseToPayDto>
{
    public async ValueTask<PromiseToPayDto> Handle(GetPromiseToPayQuery query, CancellationToken ct)
    {
        var entity = await context.PromiseToPays
            .Where(x => x.Id == query.Id)
            .Select(x => new PromiseToPayDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PromiseToPay not found");
    }
}
