using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings.GetAgentBankings;

public sealed record GetAgentBankingsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<AgentBankingsPagedResponse>;
