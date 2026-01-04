namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers;

public record FeeWaiverDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FeeWaiverSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
