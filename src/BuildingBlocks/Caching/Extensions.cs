using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FSH.Framework.Caching;

public static class Extensions
{
    public static IServiceCollection AddHeroCaching(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<CachingOptions>()
            .BindConfiguration(nameof(CachingOptions));

        // Always add memory cache for L1
        services.AddMemoryCache();

        var cacheOptions = configuration.GetSection(nameof(CachingOptions)).Get<CachingOptions>();
        if (cacheOptions == null || string.IsNullOrEmpty(cacheOptions.Redis))
        {
            // If no Redis, use memory cache for L2 as well
            services.AddDistributedMemoryCache();
            services.AddTransient<ICacheService, HybridCacheService>();
            return services;
        }

        // Use Redis for L2 cache
        services.AddStackExchangeRedisCache(options =>
        {
            ConfigurationOptions config = ConfigurationOptions.Parse(cacheOptions.Redis);
            
            // Don't abort on connect fail in production - allow graceful degradation
            config.AbortOnConnectFail = false;
            
            // Add connection retry logic
            config.ConnectRetry = 3;
            config.ConnectTimeout = 5000;
            config.SyncTimeout = 5000;
            config.AsyncTimeout = 5000;
            
            // Set default database
            config.DefaultDatabase = 0;
            
            // Add command map for better performance
            config.AllowAdmin = false;

            options.ConfigurationOptions = config;
        });

        // Register hybrid cache service
        services.AddTransient<ICacheService, HybridCacheService>();

        return services;
    }
}