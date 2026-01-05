using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.GetLoanWriteOff;

public sealed record GetLoanWriteOffQuery(Guid Id) : IQuery<LoanWriteOffDto>;
