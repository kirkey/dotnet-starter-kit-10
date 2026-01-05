using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.QrPayments.GetQrPayments;

public sealed record GetQrPaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<QrPaymentsPagedResponse>;
