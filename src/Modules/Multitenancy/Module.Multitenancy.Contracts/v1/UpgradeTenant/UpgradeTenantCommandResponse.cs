namespace FSH.Module.Multitenancy.Contracts.v1.UpgradeTenant;

public sealed record UpgradeTenantCommandResponse(DateTime NewValidity, string Tenant);