using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CashVaults;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CashVaults.GetCashVault;

public record GetCashVaultQuery(Guid Id) : IQuery<CashVaultDto>;

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
