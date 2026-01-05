using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Query to get a paginated list of members.
/// </summary>
public record GetMembersQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    bool? IsActive) : IQuery<MembersPagedResponse>;

/// <summary>
/// Paginated response for members query.
/// </summary>
public record MembersPagedResponse(
    List<MemberSummaryDto> Members,
    int TotalCount,
    int Page,
    int PageSize);
