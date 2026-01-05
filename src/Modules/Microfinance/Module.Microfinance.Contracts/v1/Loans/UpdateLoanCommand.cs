using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Loans;

/// <summary>
/// Command to update an existing loan.
/// </summary>
public record UpdateLoanCommand(Guid Id, string Name) : ICommand<Guid>;
