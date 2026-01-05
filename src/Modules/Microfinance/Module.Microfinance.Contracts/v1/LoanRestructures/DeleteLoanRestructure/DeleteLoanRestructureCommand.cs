using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.DeleteLoanRestructure;

public sealed record DeleteLoanRestructureCommand(Guid Id) : ICommand;
