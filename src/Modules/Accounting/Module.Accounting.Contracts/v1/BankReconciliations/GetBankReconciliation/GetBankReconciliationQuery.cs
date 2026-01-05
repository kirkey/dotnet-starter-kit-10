using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.GetBankReconciliation;

public record GetBankReconciliationQuery(Guid Id) : IQuery<BankReconciliationDto>;