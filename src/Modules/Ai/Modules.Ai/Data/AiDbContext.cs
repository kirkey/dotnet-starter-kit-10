using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence.Context;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Persistence;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pgvector.EntityFrameworkCore;

namespace FSH.Modules.Ai.Data;

public sealed class AiDbContext : BaseDbContext
{
    public const string Schema = "ai";

    public AiDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<AiDbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment)         : base(multiTenantContextAccessor, options, settings, environment) { }

    public DbSet<AiSource> Sources => Set<AiSource>();
    public DbSet<AiChunk> Chunks => Set<AiChunk>();
    public DbSet<AiProvider> Providers => Set<AiProvider>();
    public DbSet<AiProviderModel> ProviderModels => Set<AiProviderModel>();
    public DbSet<AiProviderSecret> ProviderSecrets => Set<AiProviderSecret>();
    public DbSet<AiChatSession> ChatSessions => Set<AiChatSession>();
    public DbSet<AiChatMessage> ChatMessages => Set<AiChatMessage>();
    public DbSet<AiDepartment> Departments => Set<AiDepartment>();
    public DbSet<AiAgent> Agents => Set<AiAgent>();
    public DbSet<AiAgentRun> AgentRuns => Set<AiAgentRun>();
    public DbSet<AiAgentSchedule> AgentSchedules => Set<AiAgentSchedule>();
    public DbSet<AiRuntime> Runtimes => Set<AiRuntime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AiDbContext).Assembly);
        // base.OnModelCreating runs LAST so BaseDbContext's auto-apply sees
        // fully-configured entities (including HasMany child types).
        base.OnModelCreating(modelBuilder);
    }
}
