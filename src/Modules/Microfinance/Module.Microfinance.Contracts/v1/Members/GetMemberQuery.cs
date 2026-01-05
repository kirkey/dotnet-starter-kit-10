using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Query to get a single member by ID.
/// </summary>
public record GetMemberQuery(Guid Id) : IQuery<MemberDto>;
