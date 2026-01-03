using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.SavingsTransactions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransaction;

public record GetSavingsTransactionQuery(Guid Id) : IQuery<SavingsTransactionDto>;

public class GetSavingsTransactionHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsTransactionQuery, SavingsTransactionDto>
{
    public async ValueTask<SavingsTransactionDto> Handle(GetSavingsTransactionQuery query, CancellationToken ct)
    {
        var entity = await context.SavingsTransactions
            .Where(x => x.Id == query.Id)
            .Select(x => new SavingsTransactionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("SavingsTransaction not found");
    }
}
