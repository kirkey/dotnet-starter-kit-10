using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;

public sealed record GetLoanDisbursementTrancheQuery(Guid Id) : IQuery<LoanDisbursementTrancheDto>;
