using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.GetSecurityDeposit;

public sealed record GetSecurityDepositQuery(Guid Id) : IQuery<SecurityDepositDto>;
