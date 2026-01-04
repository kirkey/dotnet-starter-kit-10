using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransaction;

public record GetInvestmentTransactionQuery(Guid Id) : IQuery<InvestmentTransactionDto>;

public class GetInvestmentTransactionHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentTransactionQuery, InvestmentTransactionDto>
{
    public async ValueTask<InvestmentTransactionDto> Handle(GetInvestmentTransactionQuery query, CancellationToken ct)
    {
        var entity = await context.InvestmentTransactions
            .Where(x => x.Id == query.Id)
            .Select(x => new InvestmentTransactionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InvestmentTransaction not found");
    }
}
