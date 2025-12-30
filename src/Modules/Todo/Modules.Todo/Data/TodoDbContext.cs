using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Persistence;
using FSH.Modules.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FSH.Modules.Todo.Data;

public class TodoDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;

    // DbSets
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public TodoDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<TodoDbContext> options,
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
        builder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
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
