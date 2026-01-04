using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.v1.UpdateTenantTheme;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.UpdateTenantTheme;

public sealed class UpdateTenantThemeCommandHandler(
    ITenantThemeService themeService,
    IMultiTenantContextAccessor<AppTenantInfo> tenantAccessor)
    : ICommandHandler<UpdateTenantThemeCommand>
{
    public async ValueTask<Unit> Handle(UpdateTenantThemeCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string tenantId = tenantAccessor.MultiTenantContext?.TenantInfo?.Id
                          ?? throw new InvalidOperationException("No tenant context available");

        await themeService.UpdateThemeAsync(tenantId, command.Theme, cancellationToken);

        return Unit.Value;
    }
}
