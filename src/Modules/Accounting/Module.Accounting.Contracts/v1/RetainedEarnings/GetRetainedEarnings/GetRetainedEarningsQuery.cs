using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RetainedEarnings.GetRetainedEarnings;

public sealed record GetRetainedEarningsByIdQuery(Guid Id) : IQuery<RetainedEarningsDto>;