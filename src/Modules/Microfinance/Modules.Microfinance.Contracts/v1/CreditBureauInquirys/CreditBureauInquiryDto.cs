namespace FSH.Modules.Microfinance.Contracts.v1.CreditBureauInquirys;

public record CreditBureauInquiryDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CreditBureauInquirySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
