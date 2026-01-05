using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations.GetPaymentAllocation;

public sealed record GetPaymentAllocationQuery(Guid Id) : IQuery<PaymentAllocationDto>;