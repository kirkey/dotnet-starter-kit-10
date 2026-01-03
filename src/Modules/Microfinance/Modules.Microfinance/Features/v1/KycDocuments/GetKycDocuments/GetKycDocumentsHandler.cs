using FSH.Modules.Microfinance.Contracts.v1.KycDocuments;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.KycDocuments.GetKycDocuments;

public record GetKycDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<KycDocumentsPagedResponse>;

public class GetKycDocumentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetKycDocumentsQuery, KycDocumentsPagedResponse>
{
    public async ValueTask<KycDocumentsPagedResponse> Handle(GetKycDocumentsQuery query, CancellationToken ct)
    {
        var queryable = context.KycDocuments.AsQueryable();
        
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
            .Select(x => new KycDocumentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new KycDocumentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
