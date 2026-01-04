using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.GetTenantMigrations;

public sealed record GetTenantMigrationsQuery : IQuery<IReadOnlyCollection<TenantMigrationStatusDto>>;

