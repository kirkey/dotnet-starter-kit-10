using FSH.Module.Accounting.Contracts.v1.AccountingPeriods;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.GetAccountingPeriod;


public record GetAccountingPeriodQuery(Guid Id) : IQuery<AccountingPeriodDto>;