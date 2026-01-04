using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.Dtos;
using FSH.Module.Multitenancy.Contracts.v1.GetTenantTheme;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.GetTenantTheme;

public sealed class GetTenantThemeQueryHandler(ITenantThemeService themeService)
    : IQueryHandler<GetTenantThemeQuery, TenantThemeDto>
{
    public async ValueTask<TenantThemeDto> Handle(GetTenantThemeQuery query, CancellationToken cancellationToken)
    {
        return await themeService.GetCurrentTenantThemeAsync(cancellationToken);
    }
}
