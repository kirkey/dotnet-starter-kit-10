using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

/// <summary>
/// Query to get a single loan application by ID.
/// </summary>
public record GetLoanApplicationQuery(Guid Id) : IQuery<LoanApplicationDto>;
