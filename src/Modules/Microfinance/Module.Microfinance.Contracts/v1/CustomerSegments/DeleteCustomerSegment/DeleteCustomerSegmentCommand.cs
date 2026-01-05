using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments.DeleteCustomerSegment;

public sealed record DeleteCustomerSegmentCommand(Guid Id) : ICommand;
