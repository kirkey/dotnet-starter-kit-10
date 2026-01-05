using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.GetLoanRestructure;

public sealed record GetLoanRestructureQuery(Guid Id) : IQuery<LoanRestructureDto>;
