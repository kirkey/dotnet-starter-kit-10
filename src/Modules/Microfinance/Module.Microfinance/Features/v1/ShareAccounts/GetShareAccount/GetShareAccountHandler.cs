using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareAccounts.GetShareAccount;
using FSH.Module.Microfinance.Contracts.v1.ShareAccounts;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.GetShareAccount;

public class GetShareAccountHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareAccountQuery, ShareAccountDto>
{
    public async ValueTask<ShareAccountDto> Handle(GetShareAccountQuery query, CancellationToken ct)
    {
        var entity = await context.ShareAccounts
            .Where(x => x.Id == query.Id)
            .Select(x => new ShareAccountDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ShareAccount not found");
    }
}
