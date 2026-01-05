using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.GetLoanDisbursementTranches;

public sealed record GetLoanDisbursementTranchesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanDisbursementTranchesPagedResponse>;
