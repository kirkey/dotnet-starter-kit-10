using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.ShareTransactions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.GetShareTransaction;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.GetShareTransaction;

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
