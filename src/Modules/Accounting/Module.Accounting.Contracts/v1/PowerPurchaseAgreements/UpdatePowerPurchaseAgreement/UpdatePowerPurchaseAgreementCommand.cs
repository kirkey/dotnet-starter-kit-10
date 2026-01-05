using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.UpdatePowerPurchaseAgreement;

public sealed record UpdatePowerPurchaseAgreementCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;