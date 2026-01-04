using FSH.Modules.Accounting.Contracts.v1.PostingBatches;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.GetPostingBatches;

public record GetPostingBatchesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<PostingBatchesPagedResponse>;

public record PostingBatchesPagedResponse(
    List<PostingBatchSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetPostingBatchesHandler(AccountingDbContext context) 
    : IQueryHandler<GetPostingBatchesQuery, PostingBatchesPagedResponse>
{
    public async ValueTask<PostingBatchesPagedResponse> Handle(GetPostingBatchesQuery query, CancellationToken ct)
    {
        var queryable = context.PostingBatches.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.BatchDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.BatchDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.BatchDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new PostingBatchSummaryDto(
                x.Id,
                x.Name,
                x.BatchDate,
                x.Status,
                x.EntryCount,
                x.TotalDebits,
                x.TotalCredits,
                x.IsActive))
            .ToListAsync(ct);
        
        return new PostingBatchesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
