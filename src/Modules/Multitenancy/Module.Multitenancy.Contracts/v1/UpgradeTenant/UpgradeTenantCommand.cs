using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.UpgradeTenant;

public sealed record UpgradeTenantCommand(string Tenant, DateTime ExtendedExpiryDate)
    : ICommand<UpgradeTenantCommandResponse>;