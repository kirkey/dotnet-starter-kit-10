using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Projects;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Projects.GetProject;namespace FSH.Module.Accounting.Features.v1.Projects.GetProject;

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
