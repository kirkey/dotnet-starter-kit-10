using FSH.Module.Microfinance.Contracts.v1.Documents;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Documents.GetDocuments;

public record GetDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<DocumentsPagedResponse>;

public class GetDocumentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetDocumentsQuery, DocumentsPagedResponse>
{
    public async ValueTask<DocumentsPagedResponse> Handle(GetDocumentsQuery query, CancellationToken ct)
    {
        var queryable = context.Documents.AsQueryable();
        
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
            .Select(x => new DocumentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new DocumentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
