using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccount;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccount;

public class GetInvestmentAccountHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentAccountQuery, InvestmentAccountDto>
{
    public async ValueTask<InvestmentAccountDto> Handle(GetInvestmentAccountQuery query, CancellationToken ct)
    {
        var entity = await context.InvestmentAccounts
            .Where(x => x.Id == query.Id)
            .Select(x => new InvestmentAccountDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InvestmentAccount not found");
    }
}
