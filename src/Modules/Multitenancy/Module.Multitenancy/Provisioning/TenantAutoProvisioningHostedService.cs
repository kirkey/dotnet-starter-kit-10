using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Multitenancy;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Module.Multitenancy.Provisioning;

public sealed class TenantAutoProvisioningHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TenantAutoProvisioningHostedService> _logger;
    private readonly MultitenancyOptions _options;

    public TenantAutoProvisioningHostedService(
        IServiceProvider serviceProvider,
        ILogger<TenantAutoProvisioningHostedService> logger,
        IOptions<MultitenancyOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_options.AutoProvisionOnStartup && !_options.RunTenantMigrationsOnStartup)
        {
            return;
        }

        if (!JobStorageAvailable())
        {
            _logger.LogWarning("Hangfire storage not initialized; skipping auto-provisioning enqueue.");
            return;
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        IMultiTenantStore<AppTenantInfo> tenantStore = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<AppTenantInfo>>();
        ITenantProvisioningService provisioning = scope.ServiceProvider.GetRequiredService<ITenantProvisioningService>();

        IEnumerable<AppTenantInfo> tenants = await tenantStore.GetAllAsync().ConfigureAwait(false);
        foreach (AppTenantInfo tenant in tenants)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                TenantProvisioning? latest = await provisioning.GetLatestAsync(tenant.Id, cancellationToken).ConfigureAwait(false);

                // When RunTenantMigrationsOnStartup is enabled, always re-provision to apply any new migrations
                // Otherwise, only provision if not completed yet
                bool shouldProvision = _options.RunTenantMigrationsOnStartup ||
                                       latest is null ||
                                       latest.Status != TenantProvisioningStatus.Completed;

                if (shouldProvision)
                {
                    await provisioning.StartAsync(tenant.Id, cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Enqueued provisioning for tenant {TenantId} on startup.", tenant.Id);
                }
            }
            catch (CustomException ex)
            {
                _logger.LogInformation("Provisioning already in progress or recently queued for tenant {TenantId}: {Message}", tenant.Id, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to enqueue provisioning for tenant {TenantId}", tenant.Id);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static bool JobStorageAvailable()
    {
        try
        {
            _ = JobStorage.Current;
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
