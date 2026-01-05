using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetGeneralLedger;

public record GetGeneralLedgerQuery(Guid Id) : IQuery<GeneralLedgerDto>;
