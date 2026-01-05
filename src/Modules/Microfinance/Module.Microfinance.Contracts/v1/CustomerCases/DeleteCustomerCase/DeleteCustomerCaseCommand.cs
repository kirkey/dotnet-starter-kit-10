using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases.DeleteCustomerCase;

public sealed record DeleteCustomerCaseCommand(Guid Id) : ICommand;
