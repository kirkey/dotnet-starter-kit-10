using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Services;
using Microsoft.Extensions.Logging;

namespace FSH.Module.Multitenancy.Provisioning;

public sealed class TenantProvisioningJob(
    ITenantProvisioningService provisioningService,
    IMultiTenantStore<AppTenantInfo> tenantStore,
    IMultiTenantContextSetter tenantContextSetter,
    ITenantService tenantService,
    ILogger<TenantProvisioningJob> logger)
{
    public async Task RunAsync(string tenantId, string correlationId)
    {
        AppTenantInfo tenant = await tenantStore.GetAsync(tenantId).ConfigureAwait(false)
                               ?? throw new NotFoundException($"Tenant {tenantId} not found during provisioning.");

        TenantProvisioningStepName currentStep = TenantProvisioningStepName.Database;
        try
        {
            bool runDatabase = await provisioningService.MarkRunningAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);

            tenantContextSetter.MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenant);

            if (runDatabase)
            {
                await provisioningService.MarkStepCompletedAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            }

            currentStep = TenantProvisioningStepName.Migrations;
            bool runMigrations = await provisioningService.MarkRunningAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            if (runMigrations)
            {
                await tenantService.MigrateTenantAsync(tenant, CancellationToken.None).ConfigureAwait(false);
                await provisioningService.MarkStepCompletedAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            }

            currentStep = TenantProvisioningStepName.Seeding;
            bool runSeeding = await provisioningService.MarkRunningAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            if (runSeeding)
            {
                await tenantService.SeedTenantAsync(tenant, CancellationToken.None).ConfigureAwait(false);
                await provisioningService.MarkStepCompletedAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            }

            currentStep = TenantProvisioningStepName.CacheWarm;
            bool runCacheWarm = await provisioningService.MarkRunningAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            if (runCacheWarm)
            {
                await provisioningService.MarkStepCompletedAsync(tenantId, correlationId, currentStep, CancellationToken.None).ConfigureAwait(false);
            }

            await provisioningService.MarkCompletedAsync(tenantId, correlationId, CancellationToken.None).ConfigureAwait(false);

            logger.LogInformation("Provisioned tenant {TenantId} correlation {CorrelationId}", tenantId, correlationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Provisioning failed for tenant {TenantId}", tenantId);
            await provisioningService.MarkFailedAsync(tenantId, correlationId, currentStep, ex.Message, CancellationToken.None).ConfigureAwait(false);
            throw;
        }
    }
}
