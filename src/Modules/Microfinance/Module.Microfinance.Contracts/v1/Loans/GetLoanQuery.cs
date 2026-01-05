using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Loans;

/// <summary>
/// Query to get a single loan by ID.
/// </summary>
public record GetLoanQuery(Guid Id) : IQuery<LoanDto>;
