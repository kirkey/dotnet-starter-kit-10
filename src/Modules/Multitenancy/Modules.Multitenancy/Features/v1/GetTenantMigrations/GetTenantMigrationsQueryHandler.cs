using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Multitenancy.Contracts.Dtos;
using FSH.Modules.Multitenancy.Contracts.v1.GetTenantMigrations;
using FSH.Modules.Multitenancy.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Modules.Multitenancy.Features.v1.GetTenantMigrations;

public sealed class GetTenantMigrationsQueryHandler(
    IMultiTenantStore<AppTenantInfo> tenantStore,
    IServiceScopeFactory scopeFactory)
    : IQueryHandler<GetTenantMigrationsQuery, IReadOnlyCollection<TenantMigrationStatusDto>>
{
    public async ValueTask<IReadOnlyCollection<TenantMigrationStatusDto>> Handle(
        GetTenantMigrationsQuery query,
        CancellationToken cancellationToken)
    {
        IEnumerable<AppTenantInfo> tenants = await tenantStore.GetAllAsync().ConfigureAwait(false);

        List<TenantMigrationStatusDto> tenantMigrationStatuses = new();

        foreach (AppTenantInfo tenant in tenants)
        {
            TenantMigrationStatusDto tenantStatus = new()
            {
                TenantId = tenant.Id,
                Name = tenant.Name!,
                IsActive = tenant.IsActive,
                ValidUpto = tenant.ValidUpto
            };

            try
            {
                using IServiceScope tenantScope = scopeFactory.CreateScope();

                tenantScope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                    .MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenant);

                TenantDbContext dbContext = tenantScope.ServiceProvider.GetRequiredService<TenantDbContext>();

                IEnumerable<string> appliedMigrations = await dbContext.Database
                    .GetAppliedMigrationsAsync(cancellationToken)
                    .ConfigureAwait(false);

                IEnumerable<string> pendingMigrations = await dbContext.Database
                    .GetPendingMigrationsAsync(cancellationToken)
                    .ConfigureAwait(false);

                tenantStatus.Provider = dbContext.Database.ProviderName;
                tenantStatus.LastAppliedMigration = appliedMigrations.LastOrDefault();
                tenantStatus.PendingMigrations = pendingMigrations.ToArray();
                tenantStatus.HasPendingMigrations = tenantStatus.PendingMigrations.Count > 0;
            }
            catch (Exception ex)
            {
                tenantStatus.Error = ex.Message;
            }

            tenantMigrationStatuses.Add(tenantStatus);
        }

        return tenantMigrationStatuses;
    }
}
