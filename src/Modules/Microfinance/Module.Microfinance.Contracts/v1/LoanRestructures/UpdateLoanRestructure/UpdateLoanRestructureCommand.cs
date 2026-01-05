using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.UpdateLoanRestructure;

public sealed record UpdateLoanRestructureCommand(Guid Id, string Name) : ICommand<Guid>;
