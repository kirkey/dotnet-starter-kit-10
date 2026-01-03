using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.SavingsAccounts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccount;

public record GetSavingsAccountQuery(Guid Id) : IQuery<SavingsAccountDto>;

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
