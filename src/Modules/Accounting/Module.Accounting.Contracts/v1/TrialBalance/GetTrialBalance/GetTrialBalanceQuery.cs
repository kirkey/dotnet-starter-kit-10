using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TrialBalance.GetTrialBalance;

public sealed record GetTrialBalanceQuery(Guid Id) : IQuery<TrialBalanceDto>;
