using FSH.Modules.Microfinance.Contracts.v1.Members;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Members.GetMembers;
public record GetMembersQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    bool? IsActive) : IQuery<MembersPagedResponse>;
public class GetMembersHandler(MicrofinanceDbContext context) : IQueryHandler<GetMembersQuery, MembersPagedResponse>
{
    public async ValueTask<MembersPagedResponse> Handle(GetMembersQuery query, CancellationToken ct)
    {
        var membersQuery = context.Members.AsQueryable();
        // Apply filters
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.ToLower();
            membersQuery = membersQuery.Where(m =>
                m.MemberNumber.ToLower().Contains(searchTerm) ||
                m.FirstName.ToLower().Contains(searchTerm) ||
                m.LastName.ToLower().Contains(searchTerm) ||
                (m.Email != null && m.Email.ToLower().Contains(searchTerm)) ||
                (m.PhoneNumber != null && m.PhoneNumber.Contains(searchTerm)));
        }
        if (query.IsActive.HasValue)
            membersQuery = membersQuery.Where(m => m.IsActive == query.IsActive.Value);
        var totalCount = await membersQuery.CountAsync(ct);
        var members = await membersQuery
            .OrderByDescending(m => m.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(m => new MemberSummaryDto(
                m.Id,
                m.MemberNumber,
                m.FullName,
                m.Email,
                m.PhoneNumber,
                m.IsActive,
                m.JoinDate))
            .ToListAsync(ct);
        return new MembersPagedResponse(members, totalCount, query.Page, query.PageSize);
    }
}
