namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts;

public record InsuranceProductDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InsuranceProductSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
