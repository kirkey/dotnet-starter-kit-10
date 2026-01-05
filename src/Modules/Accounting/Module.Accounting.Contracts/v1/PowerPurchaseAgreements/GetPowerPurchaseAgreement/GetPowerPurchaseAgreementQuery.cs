using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreement;

public sealed record GetPowerPurchaseAgreementQuery(Guid Id) : IQuery<PowerPurchaseAgreementDto>;