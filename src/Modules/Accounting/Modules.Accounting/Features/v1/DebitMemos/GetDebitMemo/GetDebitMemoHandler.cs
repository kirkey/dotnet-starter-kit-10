using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.DebitMemos;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.DebitMemos.GetDebitMemo;

public record GetDebitMemoQuery(Guid Id) : IQuery<DebitMemoDto>;

public class GetDebitMemoHandler(AccountingDbContext context) : IQueryHandler<GetDebitMemoQuery, DebitMemoDto>
{
    public async ValueTask<DebitMemoDto> Handle(GetDebitMemoQuery query, CancellationToken ct)
    {
        var entity = await context.DebitMemos
            .Where(x => x.Id == query.Id)
            .Select(x => new DebitMemoDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("DebitMemo not found");
    }
}
