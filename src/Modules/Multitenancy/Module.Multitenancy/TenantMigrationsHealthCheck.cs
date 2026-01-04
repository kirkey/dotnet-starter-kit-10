using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Multitenancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FSH.Module.Multitenancy;

public sealed class TenantMigrationsHealthCheck(IServiceScopeFactory scopeFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = scopeFactory.CreateScope();

        IMultiTenantStore<AppTenantInfo> tenantStore = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<AppTenantInfo>>();
        IEnumerable<AppTenantInfo> tenants = await tenantStore.GetAllAsync().ConfigureAwait(false);

        Dictionary<string, object> details = new();

        foreach (AppTenantInfo tenant in tenants)
        {
            try
            {
                using IServiceScope tenantScope = scope.ServiceProvider.CreateScope();

                tenantScope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                    .MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenant);

                TenantDbContext dbContext = tenantScope.ServiceProvider.GetRequiredService<TenantDbContext>();

                IEnumerable<string> pendingMigrations = await dbContext.Database
                    .GetPendingMigrationsAsync(cancellationToken)
                    .ConfigureAwait(false);

                bool hasPending = pendingMigrations.Any();

                details[tenant.Id] = new
                {
                    tenant.Name,
                    tenant.IsActive,
                    tenant.ValidUpto,
                    HasPendingMigrations = hasPending,
                    PendingMigrations = pendingMigrations.ToArray()
                };
            }
            catch (Exception ex)
            {
                details[tenant.Id] = new
                {
                    tenant.Name,
                    tenant.IsActive,
                    tenant.ValidUpto,
                    Error = ex.Message
                };
            }
        }

        return HealthCheckResult.Healthy("Tenant migrations status collected.", details);
    }
}
