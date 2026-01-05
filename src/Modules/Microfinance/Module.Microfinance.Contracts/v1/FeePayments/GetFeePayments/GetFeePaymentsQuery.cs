using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayments;

public sealed record GetFeePaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeePaymentsPagedResponse>;
