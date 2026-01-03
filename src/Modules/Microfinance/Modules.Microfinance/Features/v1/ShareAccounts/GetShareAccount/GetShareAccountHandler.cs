using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.ShareAccounts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ShareAccounts.GetShareAccount;

public record GetShareAccountQuery(Guid Id) : IQuery<ShareAccountDto>;

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
