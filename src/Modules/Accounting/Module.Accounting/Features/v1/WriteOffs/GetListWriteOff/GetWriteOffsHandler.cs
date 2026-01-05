using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.GetWriteOffs;

public class GetWriteOffsHandler(AccountingDbContext context) 
    : IQueryHandler<FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff.GetListWriteOffQuery, FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff.WriteOffsPagedResponse>
{
    public async ValueTask<FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff.WriteOffsPagedResponse> Handle(FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff.GetListWriteOffQuery query, CancellationToken ct)
    {
        var queryable = context.WriteOffs.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new WriteOffSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff.WriteOffsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
