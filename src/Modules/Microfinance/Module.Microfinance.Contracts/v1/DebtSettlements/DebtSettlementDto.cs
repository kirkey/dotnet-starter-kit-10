namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements;

public record DebtSettlementDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record DebtSettlementSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
