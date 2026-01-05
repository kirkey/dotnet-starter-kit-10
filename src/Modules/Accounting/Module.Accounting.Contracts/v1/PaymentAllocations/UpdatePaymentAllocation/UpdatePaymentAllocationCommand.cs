using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations.UpdatePaymentAllocation;

public sealed record UpdatePaymentAllocationCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;