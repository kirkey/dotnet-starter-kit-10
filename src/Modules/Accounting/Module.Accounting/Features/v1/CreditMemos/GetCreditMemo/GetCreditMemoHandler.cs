using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.CreditMemos;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.GetCreditMemo;

public record GetCreditMemoQuery(Guid Id) : IQuery<CreditMemoDto>;

public class GetCreditMemoHandler(AccountingDbContext context) : IQueryHandler<GetCreditMemoQuery, CreditMemoDto>
{
    public async ValueTask<CreditMemoDto> Handle(GetCreditMemoQuery query, CancellationToken ct)
    {
        var entity = await context.CreditMemos
            .Where(x => x.Id == query.Id)
            .Select(x => new CreditMemoDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CreditMemo not found");
    }
}
