using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.v1.ResetTenantTheme;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.ResetTenantTheme;

public sealed class ResetTenantThemeCommandHandler(
    ITenantThemeService themeService,
    IMultiTenantContextAccessor<AppTenantInfo> tenantAccessor)
    : ICommandHandler<ResetTenantThemeCommand>
{
    public async ValueTask<Unit> Handle(ResetTenantThemeCommand command, CancellationToken cancellationToken)
    {
        string tenantId = tenantAccessor.MultiTenantContext?.TenantInfo?.Id
                          ?? throw new InvalidOperationException("No tenant context available");

        await themeService.ResetThemeAsync(tenantId, cancellationToken);

        return Unit.Value;
    }
}
