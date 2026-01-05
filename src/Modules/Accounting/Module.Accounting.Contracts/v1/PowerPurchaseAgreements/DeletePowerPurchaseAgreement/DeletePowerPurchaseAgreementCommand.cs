using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.DeletePowerPurchaseAgreement;

public sealed record DeletePowerPurchaseAgreementCommand(Guid Id) : ICommand;