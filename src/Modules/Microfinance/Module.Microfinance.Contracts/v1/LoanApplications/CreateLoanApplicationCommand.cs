using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

/// <summary>
/// Command to create a new loan application.
/// </summary>
public record CreateLoanApplicationCommand(string Name) : ICommand<Guid>;
