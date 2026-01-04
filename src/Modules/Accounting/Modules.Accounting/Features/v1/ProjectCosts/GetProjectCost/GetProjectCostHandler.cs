using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.ProjectCosts;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.ProjectCosts.GetProjectCost;

public record GetProjectCostQuery(Guid Id) : IQuery<ProjectCostDto>;

public class GetProjectCostHandler(AccountingDbContext context) : IQueryHandler<GetProjectCostQuery, ProjectCostDto>
{
    public async ValueTask<ProjectCostDto> Handle(GetProjectCostQuery query, CancellationToken ct)
    {
        var entity = await context.ProjectCosts
            .Where(x => x.Id == query.Id)
            .Select(x => new ProjectCostDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ProjectCost not found");
    }
}
