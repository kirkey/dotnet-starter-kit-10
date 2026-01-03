namespace FSH.Modules.Microfinance.Contracts.v1.LoanProducts;

public record LoanProductDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanProductSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
