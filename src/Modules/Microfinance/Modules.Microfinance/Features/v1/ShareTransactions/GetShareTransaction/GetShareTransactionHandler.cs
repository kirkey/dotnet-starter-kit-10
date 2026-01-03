using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.ShareTransactions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ShareTransactions.GetShareTransaction;

public record GetShareTransactionQuery(Guid Id) : IQuery<ShareTransactionDto>;

public class GetShareTransactionHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareTransactionQuery, ShareTransactionDto>
{
    public async ValueTask<ShareTransactionDto> Handle(GetShareTransactionQuery query, CancellationToken ct)
    {
        var entity = await context.ShareTransactions
            .Where(x => x.Id == query.Id)
            .Select(x => new ShareTransactionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ShareTransaction not found");
    }
}
