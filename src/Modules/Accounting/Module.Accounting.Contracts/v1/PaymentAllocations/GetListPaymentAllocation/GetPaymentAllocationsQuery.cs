using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations.GetListPaymentAllocation;

public sealed record GetPaymentAllocationsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PaymentAllocationsPagedResponse>;

public sealed record PaymentAllocationsPagedResponse(
    List<PaymentAllocationSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);