using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Budgets;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Budgets.GetBudget;

public record GetBudgetQuery(Guid Id) : IQuery<BudgetDto>;

public class GetBudgetHandler(AccountingDbContext context) : IQueryHandler<GetBudgetQuery, BudgetDto>
{
    public async ValueTask<BudgetDto> Handle(GetBudgetQuery query, CancellationToken ct)
    {
        var entity = await context.Budgets
            .Where(x => x.Id == query.Id)
            .Select(x => new BudgetDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Budget not found");
    }
}
