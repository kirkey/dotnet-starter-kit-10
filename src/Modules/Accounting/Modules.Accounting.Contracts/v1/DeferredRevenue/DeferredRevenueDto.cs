namespace FSH.Modules.Accounting.Contracts.v1.DeferredRevenue;

public record DeferredRevenueDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record DeferredRevenueSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
