using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.CreateLoanRestructure;

public sealed record CreateLoanRestructureCommand(string Name) : ICommand<Guid>;
