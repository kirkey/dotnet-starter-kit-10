using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Budgets;
using FSH.Module.Accounting.Contracts.v1.Budgets.GetBudget;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Budgets.GetBudget;

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
