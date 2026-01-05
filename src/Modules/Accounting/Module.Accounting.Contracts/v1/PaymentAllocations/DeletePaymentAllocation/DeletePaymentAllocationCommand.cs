using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations.DeletePaymentAllocation;

public sealed record DeletePaymentAllocationCommand(Guid Id) : ICommand;