using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.MobileTransactions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.GetMobileTransaction;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.GetMobileTransaction;

public class GetMobileTransactionHandler(MicrofinanceDbContext context) : IQueryHandler<GetMobileTransactionQuery, MobileTransactionDto>
{
    public async ValueTask<MobileTransactionDto> Handle(GetMobileTransactionQuery query, CancellationToken ct)
    {
        var entity = await context.MobileTransactions
            .Where(x => x.Id == query.Id)
            .Select(x => new MobileTransactionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("MobileTransaction not found");
    }
}
