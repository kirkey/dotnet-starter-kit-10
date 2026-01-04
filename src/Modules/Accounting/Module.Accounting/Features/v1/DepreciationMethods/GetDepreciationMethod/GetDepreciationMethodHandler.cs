using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.DepreciationMethods;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.GetDepreciationMethod;

public record GetDepreciationMethodQuery(Guid Id) : IQuery<DepreciationMethodDto>;

public class GetDepreciationMethodHandler(AccountingDbContext context) : IQueryHandler<GetDepreciationMethodQuery, DepreciationMethodDto>
{
    public async ValueTask<DepreciationMethodDto> Handle(GetDepreciationMethodQuery query, CancellationToken ct)
    {
        var entity = await context.DepreciationMethods
            .Where(x => x.Id == query.Id)
            .Select(x => new DepreciationMethodDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("DepreciationMethod not found");
    }
}
