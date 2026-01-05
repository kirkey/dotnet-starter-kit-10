using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CashVaults.GetCashVault;
using FSH.Module.Microfinance.Contracts.v1.CashVaults;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.GetCashVault;

public class GetCashVaultHandler(MicrofinanceDbContext context) : IQueryHandler<GetCashVaultQuery, CashVaultDto>
{
    public async ValueTask<CashVaultDto> Handle(GetCashVaultQuery query, CancellationToken ct)
    {
        var entity = await context.CashVaults
            .Where(x => x.Id == query.Id)
            .Select(x => new CashVaultDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CashVault not found");
    }
}
