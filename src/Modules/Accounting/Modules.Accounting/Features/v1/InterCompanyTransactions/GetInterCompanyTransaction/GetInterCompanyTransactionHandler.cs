using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.InterCompanyTransactions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.GetInterCompanyTransaction;

public record GetInterCompanyTransactionQuery(Guid Id) : IQuery<InterCompanyTransactionDto>;

public class GetInterCompanyTransactionHandler(AccountingDbContext context) : IQueryHandler<GetInterCompanyTransactionQuery, InterCompanyTransactionDto>
{
    public async ValueTask<InterCompanyTransactionDto> Handle(GetInterCompanyTransactionQuery query, CancellationToken ct)
    {
        var entity = await context.InterCompanyTransactions
            .Where(x => x.Id == query.Id)
            .Select(x => new InterCompanyTransactionDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InterCompanyTransaction not found");
    }
}
