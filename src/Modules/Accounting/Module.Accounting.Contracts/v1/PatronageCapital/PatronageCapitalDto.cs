namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital;

public record PatronageCapitalDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PatronageCapitalSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);