using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.DeleteDeferredRevenue;

public sealed record DeleteDeferredRevenueCommand(Guid Id) : ICommand;