using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.UpdateDeferredRevenue;

public sealed record UpdateDeferredRevenueCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;