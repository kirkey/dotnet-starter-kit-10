namespace FSH.Modules.Microfinance.Contracts.v1.PromiseToPays;

public record PromiseToPayDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PromiseToPaySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
