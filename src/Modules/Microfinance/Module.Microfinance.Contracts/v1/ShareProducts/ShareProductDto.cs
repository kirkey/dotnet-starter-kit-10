namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts;

public record ShareProductDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ShareProductSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
