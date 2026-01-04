using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.WriteOffs;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.WriteOffs.GetWriteOff;

public record GetWriteOffQuery(Guid Id) : IQuery<WriteOffDto>;

public class GetWriteOffHandler(AccountingDbContext context) : IQueryHandler<GetWriteOffQuery, WriteOffDto>
{
    public async ValueTask<WriteOffDto> Handle(GetWriteOffQuery query, CancellationToken ct)
    {
        var entity = await context.WriteOffs
            .Where(x => x.Id == query.Id)
            .Select(x => new WriteOffDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("WriteOff not found");
    }
}
