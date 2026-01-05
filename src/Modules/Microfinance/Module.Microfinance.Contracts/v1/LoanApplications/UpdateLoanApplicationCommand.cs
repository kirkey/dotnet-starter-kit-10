using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

/// <summary>
/// Command to update an existing loan application.
/// </summary>
public record UpdateLoanApplicationCommand(Guid Id, string Name) : ICommand<Guid>;
