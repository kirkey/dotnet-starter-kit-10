using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.GetDeferredRevenue;

public sealed record GetDeferredRevenueByIdQuery(Guid Id) : IQuery<DeferredRevenueDto>;
