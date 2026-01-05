namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;

public record InterconnectionAgreementDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InterconnectionAgreementSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);