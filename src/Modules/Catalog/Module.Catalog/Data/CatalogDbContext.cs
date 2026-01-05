using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using CategoryEntity = FSH.Module.Catalog.Domain.Category;
using BrandEntity = FSH.Module.Catalog.Domain.Brand;
using ProductEntity = FSH.Module.Catalog.Domain.Product;

namespace FSH.Module.Catalog.Data;

/// <summary>
/// Entity Framework Core DbContext for the Catalog module.
/// 
/// **Purpose:**
/// Manages all database operations for Category, Brand, and Product entities.
/// Supports multi-tenancy through tenant-specific connection strings.
/// 
/// **Features:**
/// - Multi-tenant database isolation through connection string per tenant
/// - Automatic application of entity configurations
/// - Proper DbSet definitions for all catalog entities
/// - Support for EF Core migrations
/// </summary>
public class CatalogDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;

    /// <summary>Gets the DbSet for Category entities.</summary>
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    
    /// <summary>Gets the DbSet for Brand entities.</summary>
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    
    /// <summary>Gets the DbSet for Product entities.</summary>
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public CatalogDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<CatalogDbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment) : base(options)
    {
        ArgumentNullException.ThrowIfNull(multiTenantContextAccessor);
        ArgumentNullException.ThrowIfNull(settings);

        _environment = environment;
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        // Apply entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure tenant-specific connection
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.ConfigureHeroDatabase(
                _settings.Provider,
                TenantInfo.ConnectionString,
                _settings.MigrationsAssembly,
                _environment.IsDevelopment());
        }
    }
}
