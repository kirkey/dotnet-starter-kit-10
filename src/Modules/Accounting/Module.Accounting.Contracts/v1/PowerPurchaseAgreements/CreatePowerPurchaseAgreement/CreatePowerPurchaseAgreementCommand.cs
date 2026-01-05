using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

public sealed record CreatePowerPurchaseAgreementCommand(string Name, string? Description = null) : ICommand<Guid>;