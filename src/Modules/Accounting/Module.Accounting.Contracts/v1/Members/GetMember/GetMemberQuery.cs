using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Members.GetMember;

public sealed record GetMemberQuery(Guid Id) : IQuery<MemberDto>;
