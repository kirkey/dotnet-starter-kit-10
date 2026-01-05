using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.WriteOffs.GetWriteOff;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.GetWriteOff;

public class GetWriteOffHandler(AccountingDbContext context) : IQueryHandler<FSH.Module.Accounting.Contracts.v1.WriteOffs.GetWriteOff.GetWriteOffQuery, FSH.Module.Accounting.Contracts.v1.WriteOffs.WriteOffDto>
{
    public async ValueTask<FSH.Module.Accounting.Contracts.v1.WriteOffs.WriteOffDto> Handle(FSH.Module.Accounting.Contracts.v1.WriteOffs.GetWriteOff.GetWriteOffQuery query, CancellationToken ct)
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
