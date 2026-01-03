namespace FSH.Modules.Microfinance.Contracts.v1.InvestmentProducts;

public record InvestmentProductDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InvestmentProductSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
