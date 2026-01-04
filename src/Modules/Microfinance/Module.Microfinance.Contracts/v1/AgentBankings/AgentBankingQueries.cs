namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings;

public record GetAgentBankingQuery(Guid Id);
public record GetAgentBankingsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record AgentBankingsPagedResponse(List<AgentBankingSummaryDto> Items, int TotalCount, int Page, int PageSize);
