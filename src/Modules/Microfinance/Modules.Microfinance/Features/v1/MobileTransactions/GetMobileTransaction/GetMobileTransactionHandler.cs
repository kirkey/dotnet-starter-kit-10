using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.MobileTransactions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.MobileTransactions.GetMobileTransaction;

public record GetMobileTransactionQuery(Guid Id) : IQuery<MobileTransactionDto>;

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
