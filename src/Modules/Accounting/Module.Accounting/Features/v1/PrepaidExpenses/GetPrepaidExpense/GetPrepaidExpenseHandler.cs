using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.GetPrepaidExpense;

public record GetPrepaidExpenseQuery(Guid Id) : IQuery<PrepaidExpenseDto>;

public class GetPrepaidExpenseHandler(AccountingDbContext context) : IQueryHandler<GetPrepaidExpenseQuery, PrepaidExpenseDto>
{
    public async ValueTask<PrepaidExpenseDto> Handle(GetPrepaidExpenseQuery query, CancellationToken ct)
    {
        var entity = await context.PrepaidExpenses
            .Where(x => x.Id == query.Id)
            .Select(x => new PrepaidExpenseDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PrepaidExpense not found");
    }
}
