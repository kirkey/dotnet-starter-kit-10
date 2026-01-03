namespace FSH.Modules.Microfinance.Contracts.v1.CreditScores;

public record CreditScoreDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CreditScoreSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
