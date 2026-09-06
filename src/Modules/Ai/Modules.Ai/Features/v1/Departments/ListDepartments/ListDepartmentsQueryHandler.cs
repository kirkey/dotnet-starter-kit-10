using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Departments;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Departments.ListDepartments;

public sealed class ListDepartmentsQueryHandler(AiDbContext db)
    : IQueryHandler<ListDepartmentsQuery, IReadOnlyList<AiDepartmentDto>>
{
    public async ValueTask<IReadOnlyList<AiDepartmentDto>> Handle(ListDepartmentsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .Select(d => new AiDepartmentDto(
                d.Id,
                d.Name,
                d.Description,
                db.Agents.Count(a => a.DepartmentId == d.Id)))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
