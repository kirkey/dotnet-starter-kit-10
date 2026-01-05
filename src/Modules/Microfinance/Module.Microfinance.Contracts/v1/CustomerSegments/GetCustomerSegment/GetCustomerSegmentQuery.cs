using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments.GetCustomerSegment;

public sealed record GetCustomerSegmentQuery(Guid Id) : IQuery<CustomerSegmentDto>;
