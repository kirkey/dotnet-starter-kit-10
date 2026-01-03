namespace FSH.Modules.Microfinance.Contracts.v1.Members;

public record GetMemberQuery(Guid MemberId);

public record GetMembersQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    bool? IsActive);

public record MembersPagedResponse(
    List<MemberSummaryDto> Members,
    int TotalCount,
    int Page,
    int PageSize);
