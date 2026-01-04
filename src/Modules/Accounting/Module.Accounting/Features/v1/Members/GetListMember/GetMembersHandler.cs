using FSH.Module.Accounting.Contracts.v1.Members;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Members.GetMembers;

public record GetMembersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<MembersPagedResponse>;

public record MembersPagedResponse(
    List<MemberSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetMembersHandler(AccountingDbContext context) 
    : IQueryHandler<GetMembersQuery, MembersPagedResponse>
{
    public async ValueTask<MembersPagedResponse> Handle(GetMembersQuery query, CancellationToken ct)
    {
        var queryable = context.Members.AsQueryable();
        
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
            .Select(x => new MemberSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MembersPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
