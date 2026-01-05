using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments.UpdateCustomerSegment;

public sealed record UpdateCustomerSegmentCommand(Guid Id, string Name) : ICommand<Guid>;
