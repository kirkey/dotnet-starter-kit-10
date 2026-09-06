using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Ai.Data.Configurations;

public sealed class AiDepartmentConfiguration : IEntityTypeConfiguration<AiDepartment>
{
    public void Configure(EntityTypeBuilder<AiDepartment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Departments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1024);

        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiAgentConfiguration : IEntityTypeConfiguration<AiAgent>
{
    public void Configure(EntityTypeBuilder<AiAgent> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Agents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Instructions).IsRequired().HasColumnType("text");
        builder.Property(x => x.SkillsJson).IsRequired().HasMaxLength(4096);
        builder.Property(x => x.RuntimeBinding).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Model).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Variant).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.AccessMode).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.AccessUserIdsJson).IsRequired().HasMaxLength(4096);

        builder.HasIndex(x => x.DepartmentId);
        builder.HasIndex(x => x.IsArchived);
        builder.HasIndex(x => new { x.DepartmentId, x.Name }).IsUnique();

        // Agents reference their department by ID only (aggregate-root rule); the FK stays
        // restrictive so departments with agents cannot be deleted out from under them.
        builder.HasOne<AiDepartment>()
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiAgentRunConfiguration : IEntityTypeConfiguration<AiAgentRun>
{
    public void Configure(EntityTypeBuilder<AiAgentRun> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("AgentRuns");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Trigger).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.Input).IsRequired().HasColumnType("text");
        builder.Property(x => x.Output).HasColumnType("text");
        builder.Property(x => x.Error).HasMaxLength(2048);

        builder.HasIndex(x => x.AgentId);
        builder.HasIndex(x => x.Status);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiAgentScheduleConfiguration : IEntityTypeConfiguration<AiAgentSchedule>
{
    public void Configure(EntityTypeBuilder<AiAgentSchedule> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("AgentSchedules");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Cron).IsRequired().HasMaxLength(100);
        builder.Property(x => x.TaskType).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.TaskConfigJson).IsRequired().HasMaxLength(4096);
        builder.Property(x => x.LastContentHash).HasMaxLength(256);
        builder.Property(x => x.WebhookToken).IsRequired().HasMaxLength(64);

        builder.HasIndex(x => x.AgentId);
        builder.HasIndex(x => x.IsEnabled);
        builder.HasIndex(x => x.WebhookToken).IsUnique();
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiRuntimeConfiguration : IEntityTypeConfiguration<AiRuntime>
{
    public void Configure(EntityTypeBuilder<AiRuntime> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Runtimes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Family).IsRequired().HasMaxLength(100);
        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.DetectedVersion).HasMaxLength(100);
        builder.Property(x => x.Source).IsRequired().HasMaxLength(32);

        builder.HasIndex(x => x.Family).IsUnique();
        builder.Ignore(x => x.DomainEvents);
    }
}
