namespace FSH.Modules.Microfinance.Contracts.v1.KycDocuments;

public record KycDocumentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record KycDocumentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
