using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Loans;

/// <summary>
/// Command to delete a loan.
/// </summary>
public record DeleteLoanCommand(Guid Id) : ICommand<Unit>;
