using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.MobileWallets;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.MobileWallets.GetMobileWallet;

public record GetMobileWalletQuery(Guid Id) : IQuery<MobileWalletDto>;

public class GetMobileWalletHandler(MicrofinanceDbContext context) : IQueryHandler<GetMobileWalletQuery, MobileWalletDto>
{
    public async ValueTask<MobileWalletDto> Handle(GetMobileWalletQuery query, CancellationToken ct)
    {
        var entity = await context.MobileWallets
            .Where(x => x.Id == query.Id)
            .Select(x => new MobileWalletDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("MobileWallet not found");
    }
}
