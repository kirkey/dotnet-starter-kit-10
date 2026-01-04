using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.UpdateTenantTheme;

public sealed record UpdateTenantThemeCommand(TenantThemeDto Theme) : ICommand;
