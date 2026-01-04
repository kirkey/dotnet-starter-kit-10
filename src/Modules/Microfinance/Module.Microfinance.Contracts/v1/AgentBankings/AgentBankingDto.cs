namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings;

public record AgentBankingDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AgentBankingSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
