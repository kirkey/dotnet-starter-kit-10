using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.GetAccountReconciliation;

public record GetAccountReconciliationQuery(Guid Id) : IQuery<AccountReconciliationDto>;