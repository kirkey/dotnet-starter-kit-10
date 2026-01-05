namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements;

public record PowerPurchaseAgreementDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PowerPurchaseAgreementSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);