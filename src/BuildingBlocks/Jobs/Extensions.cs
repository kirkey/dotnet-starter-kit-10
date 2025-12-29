using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Framework.Shared.Persistence;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FSH.Framework.Jobs;

public static class Extensions
{
    public static IServiceCollection AddHeroJobs(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<HangfireOptions>()
            .BindConfiguration(nameof(HangfireOptions));

        services.AddHangfireServer(options =>
        {
            options.HeartbeatInterval = TimeSpan.FromSeconds(30);
            options.Queues = ["default", "email"];
            options.WorkerCount = 5;
            options.SchedulePollingInterval = TimeSpan.FromSeconds(30);
        });

        services.AddHangfire((provider, config) =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            DatabaseOptions dbOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>()
                                        ?? throw new CustomException("Database options not found");

            switch (dbOptions.Provider.ToUpperInvariant())
            {
                case DbProviders.PostgreSQL:
                    // Clean up stale locks before configuring Hangfire
                    CleanupStaleLocks(dbOptions.ConnectionString, provider);

                    config.UsePostgreSqlStorage(o =>
                    {
                        o.UseNpgsqlConnection(dbOptions.ConnectionString);
                    });
                    break;

                case DbProviders.MSSQL:
                    config.UseSqlServerStorage(dbOptions.ConnectionString);
                    break;

                default:
                    throw new CustomException($"Hangfire storage provider {dbOptions.Provider} is not supported");
            }

            config.UseFilter(new FshJobFilter(provider));
            config.UseFilter(new LogJobFilter());
            config.UseFilter(new HangfireTelemetryFilter());
        });

        services.AddTransient<IJobService, HangfireService>();

        return services;
    }

    private static void CleanupStaleLocks(string connectionString, IServiceProvider provider)
    {
        ILogger? logger = provider.GetService<ILoggerFactory>()?.CreateLogger("Hangfire");

        try
        {
            using NpgsqlConnection connection = new(connectionString);
            connection.Open();

            // Delete locks older than 5 minutes (stale from crashed instances)
            using NpgsqlCommand cmd = new(
                "DELETE FROM hangfire.lock WHERE acquired < NOW() - INTERVAL '5 minutes'",
                connection);

            int deleted = cmd.ExecuteNonQuery();
            if (deleted > 0)
            {
                logger?.LogWarning("Cleaned up {Count} stale Hangfire locks", deleted);
            }
        }
        catch (Exception ex)
        {
            // Don't fail startup if cleanup fails - the lock might not exist yet
            logger?.LogDebug(ex, "Could not cleanup stale Hangfire locks (table may not exist yet)");
        }
    }


    public static IApplicationBuilder UseHeroJobDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(config);

        HangfireOptions hangfireOptions = config.GetSection(nameof(HangfireOptions)).Get<HangfireOptions>() ?? new HangfireOptions();
        DashboardOptions dashboardOptions = new();
        dashboardOptions.AppPath = "/";
        dashboardOptions.Authorization = new[]
        {
           new HangfireCustomBasicAuthenticationFilter
           {
                User = hangfireOptions.UserName!,
                Pass = hangfireOptions.Password!
           }
        };

        return app.UseHangfireDashboard(hangfireOptions.Route, dashboardOptions);
    }
}
