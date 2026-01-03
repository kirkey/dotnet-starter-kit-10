namespace FSH.Modules.Microfinance.Contracts.v1.LoanSchedules;

public record LoanScheduleDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanScheduleSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
