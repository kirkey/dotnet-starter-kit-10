using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.GetLoanOfficerTarget;

public sealed record GetLoanOfficerTargetQuery(Guid Id) : IQuery<LoanOfficerTargetDto>;
