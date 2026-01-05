using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Loans;

/// <summary>
/// Command to create a new loan.
/// </summary>
public record CreateLoanCommand(string Name) : ICommand<Guid>;
