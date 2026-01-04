namespace FSH.Modules.Accounting.Contracts.v1.Vendors;

public record VendorDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record VendorSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
