namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs;

public record LoanWriteOffDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanWriteOffSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
