using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.GetTenantTheme;

public sealed record GetTenantThemeQuery : IQuery<TenantThemeDto>;
