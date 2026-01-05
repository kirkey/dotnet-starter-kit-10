using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.GetSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransaction;

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
