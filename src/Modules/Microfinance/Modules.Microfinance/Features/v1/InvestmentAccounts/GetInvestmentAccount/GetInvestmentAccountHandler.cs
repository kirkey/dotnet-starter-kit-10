using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.InvestmentAccounts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccount;

public record GetInvestmentAccountQuery(Guid Id) : IQuery<InvestmentAccountDto>;

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
