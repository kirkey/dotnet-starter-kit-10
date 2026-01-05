using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

/// <summary>
/// Command to delete a loan application.
/// </summary>
public record DeleteLoanApplicationCommand(Guid Id) : ICommand<Unit>;
