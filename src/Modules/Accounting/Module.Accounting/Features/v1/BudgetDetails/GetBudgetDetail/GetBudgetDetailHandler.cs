using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails.GetBudgetDetail;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.GetBudgetDetail;


public class GetBudgetDetailHandler(AccountingDbContext context) : IQueryHandler<GetBudgetDetailQuery, BudgetDetailDto>
{
    public async ValueTask<BudgetDetailDto> Handle(GetBudgetDetailQuery query, CancellationToken ct)
    {
        var entity = await context.BudgetDetails
            .Where(x => x.Id == query.Id)
            .Select(x => new BudgetDetailDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("BudgetDetail not found");
    }
}
