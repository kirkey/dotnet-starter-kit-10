using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments.CreateCustomerSegment;

public sealed record CreateCustomerSegmentCommand(string Name) : ICommand<Guid>;
