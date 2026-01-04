namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates;

public record CommunicationTemplateDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CommunicationTemplateSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
