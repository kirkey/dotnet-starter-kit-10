using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases.UpdateCustomerCase;

public sealed record UpdateCustomerCaseCommand(Guid Id, string Name) : ICommand<Guid>;
