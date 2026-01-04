using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.GetWriteOffs;

public record GetWriteOffsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<WriteOffsPagedResponse>;

public record WriteOffsPagedResponse(
    List<WriteOffSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetWriteOffsHandler(AccountingDbContext context) 
    : IQueryHandler<GetWriteOffsQuery, WriteOffsPagedResponse>
{
    public async ValueTask<WriteOffsPagedResponse> Handle(GetWriteOffsQuery query, CancellationToken ct)
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
        
        return new WriteOffsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
