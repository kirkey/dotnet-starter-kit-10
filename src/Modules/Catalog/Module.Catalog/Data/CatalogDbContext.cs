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
/// Configured to work with PostgreSQL, MSSQL, or other EF Core supported databases.
/// 
/// **Features:**
/// - Multi-tenant database isolation through connection string per tenant
/// - Automatic application of entity configurations
/// - Proper DbSet definitions for Category, Brand, and Product entities
/// - Support for EF Core migrations
/// 
/// **Configuration:**
/// - Applies all entity configurations from the assembly
/// - Configures database provider based on DatabaseOptions settings
/// - Tenant-specific connection string from MultiTenantContext
/// </summary>
public class CatalogDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Gets the DbSet for Category entities.
    /// </summary>
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    
    /// <summary>
    /// Gets the DbSet for Brand entities.
    /// </summary>
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    
    /// <summary>
    /// Gets the DbSet for Product entities.
    /// </summary>
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    /// <summary>
    /// Initializes a new instance of the CatalogDbContext.
    /// 
    /// Sets up the context with multi-tenant awareness and database configuration options.
    /// </summary>
    /// <param name="multiTenantContextAccessor">Provides access to current tenant information.</param>
    /// <param name="options">EF Core DbContext options.</param>
    /// <param name="settings">Database provider and configuration settings.</param>
    /// <param name="environment">The host environment (Development, Production, etc.).</param>
    /// <exception cref="ArgumentNullException">Thrown if multiTenantContextAccessor or settings is null.</exception>
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

    /// <summary>
    /// Configures the data model using Entity Framework model builder.
    /// 
    /// Applies all entity configurations found in this assembly,
    /// allowing centralized mapping configuration for Category, Brand, and Product entities.
    /// </summary>
    /// <param name="builder">The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        // Apply entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }

    /// <summary>
    /// Configures the database connection and provider options.
    /// 
    /// Sets up the database provider (PostgreSQL, MSSQL, etc.) based on configuration
    /// and uses the tenant-specific connection string for data isolation.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the database.</param>
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
