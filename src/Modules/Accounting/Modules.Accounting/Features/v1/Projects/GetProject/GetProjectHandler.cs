using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Projects;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Projects.GetProject;

public record GetProjectQuery(Guid Id) : IQuery<ProjectDto>;

public class GetProjectHandler(AccountingDbContext context) : IQueryHandler<GetProjectQuery, ProjectDto>
{
    public async ValueTask<ProjectDto> Handle(GetProjectQuery query, CancellationToken ct)
    {
        var entity = await context.Projects
            .Where(x => x.Id == query.Id)
            .Select(x => new ProjectDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Project not found");
    }
}
