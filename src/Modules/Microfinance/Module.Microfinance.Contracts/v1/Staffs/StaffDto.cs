namespace FSH.Module.Microfinance.Contracts.v1.Staffs;

public record StaffDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record StaffSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
