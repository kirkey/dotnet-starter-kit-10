using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases.CreateCustomerCase;

public sealed record CreateCustomerCaseCommand(string Name) : ICommand<Guid>;
