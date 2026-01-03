namespace FSH.Modules.Microfinance.Contracts.v1.FeePayments;

public record GetFeePaymentQuery(Guid Id);
public record GetFeePaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record FeePaymentsPagedResponse(List<FeePaymentSummaryDto> Items, int TotalCount, int Page, int PageSize);
