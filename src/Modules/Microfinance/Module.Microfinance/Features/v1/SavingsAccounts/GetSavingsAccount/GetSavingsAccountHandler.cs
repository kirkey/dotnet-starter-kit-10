using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccount;

public class GetSavingsAccountHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsAccountQuery, SavingsAccountDto>
{
    public async ValueTask<SavingsAccountDto> Handle(GetSavingsAccountQuery query, CancellationToken ct)
    {
        var entity = await context.SavingsAccounts
            .Where(x => x.Id == query.Id)
            .Select(x => new SavingsAccountDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("SavingsAccount not found");
    }
}
