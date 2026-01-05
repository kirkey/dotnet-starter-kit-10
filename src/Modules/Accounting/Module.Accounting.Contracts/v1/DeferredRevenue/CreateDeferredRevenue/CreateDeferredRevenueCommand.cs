using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.CreateDeferredRevenue;

public sealed record CreateDeferredRevenueCommand(string Name, string? Description = null) : ICommand<Guid>;