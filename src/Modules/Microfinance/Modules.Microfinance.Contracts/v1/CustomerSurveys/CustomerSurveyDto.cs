namespace FSH.Modules.Microfinance.Contracts.v1.CustomerSurveys;

public record CustomerSurveyDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CustomerSurveySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
