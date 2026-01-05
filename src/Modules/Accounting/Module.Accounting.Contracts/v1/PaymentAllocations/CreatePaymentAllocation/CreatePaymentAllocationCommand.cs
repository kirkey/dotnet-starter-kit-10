using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations.CreatePaymentAllocation;

public sealed record CreatePaymentAllocationCommand(string Name, string? Description = null) : ICommand<Guid>;