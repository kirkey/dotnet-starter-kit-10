using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Multitenancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Multitenancy.Provisioning;

/// <summary>
/// Initializes the tenant catalog database and seeds the root tenant on startup.
/// </summary>
public sealed class TenantStoreInitializerHostedService(
    IServiceProvider serviceProvider,
    ILogger<TenantStoreInitializerHostedService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        TenantDbContext tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
        await tenantDbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Applied tenant catalog migrations.");

        if (await tenantDbContext.TenantInfo.FindAsync([MultitenancyConstants.Root.Id], cancellationToken).ConfigureAwait(false) is null)
        {
            AppTenantInfo rootTenant = new(
                MultitenancyConstants.Root.Id,
                MultitenancyConstants.Root.Name,
                string.Empty,
                MultitenancyConstants.Root.EmailAddress,
                issuer: MultitenancyConstants.Root.Issuer);

            DateTime validUpto = DateTime.UtcNow.AddYears(1);
            rootTenant.SetValidity(validUpto);
            await tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken).ConfigureAwait(false);
            await tenantDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Seeded root tenant.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
