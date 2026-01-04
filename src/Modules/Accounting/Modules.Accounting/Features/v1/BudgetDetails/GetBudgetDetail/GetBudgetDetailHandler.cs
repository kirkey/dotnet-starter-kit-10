using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.BudgetDetails;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.BudgetDetails.GetBudgetDetail;

public record GetBudgetDetailQuery(Guid Id) : IQuery<BudgetDetailDto>;

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
