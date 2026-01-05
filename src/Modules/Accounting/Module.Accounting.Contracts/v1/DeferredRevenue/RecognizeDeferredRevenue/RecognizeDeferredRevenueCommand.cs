using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.RecognizeDeferredRevenue;

public sealed record RecognizeDeferredRevenueCommand(Guid Id) : ICommand;