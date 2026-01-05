using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantor;

public sealed record GetLoanGuarantorQuery(Guid Id) : IQuery<LoanGuarantorDto>;
