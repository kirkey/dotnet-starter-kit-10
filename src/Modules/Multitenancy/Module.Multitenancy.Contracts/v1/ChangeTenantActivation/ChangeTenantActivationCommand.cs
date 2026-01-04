using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.ChangeTenantActivation;

public sealed record ChangeTenantActivationCommand(string TenantId, bool IsActive)
    : ICommand<TenantLifecycleResultDto>;