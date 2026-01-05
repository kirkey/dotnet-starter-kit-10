using Mediator;
using FSH.Module.Accounting.Contracts.v1.Members;

namespace FSH.Module.Accounting.Contracts.v1.Members.GetListMember;

public sealed record GetMembersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<MembersPagedResponse>;

public sealed record MembersPagedResponse(
    List<MemberSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
