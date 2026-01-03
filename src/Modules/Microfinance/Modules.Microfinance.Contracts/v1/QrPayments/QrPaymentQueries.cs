namespace FSH.Modules.Microfinance.Contracts.v1.QrPayments;

public record GetQrPaymentQuery(Guid Id);
public record GetQrPaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record QrPaymentsPagedResponse(List<QrPaymentSummaryDto> Items, int TotalCount, int Page, int PageSize);
