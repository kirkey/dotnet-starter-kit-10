using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FSH.Module.Multitenancy.Data;

public sealed class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        // Design-time factory: read configuration (appsettings + env vars) to decide provider and connection.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string provider = configuration["DatabaseOptions:Provider"] ?? "POSTGRESQL";
        string connectionString = configuration["DatabaseOptions:ConnectionString"]
                                  ?? "Host=localhost;Database=fsh-tenant;Username=postgres;Password=postgres";
        string migrationsAssembly = configuration["DatabaseOptions:MigrationsAssembly"] ?? "FSH.Basic.Migrations.PostgreSQL";

        DbContextOptionsBuilder<TenantDbContext> optionsBuilder = new();

        switch (provider.ToUpperInvariant())
        {
            case "POSTGRESQL":
                optionsBuilder.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(migrationsAssembly));
                break;
            default:
                throw new NotSupportedException($"Database provider '{provider}' is not supported for TenantDbContext migrations.");
        }

        return new TenantDbContext(optionsBuilder.Options);
    }
}
