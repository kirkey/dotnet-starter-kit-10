using System.Text.Json;
using FSH.Framework.Core.Domain;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// A department agent: identity + instructions + skills + runtime binding + model/variant.
/// Archived agents disappear from pickers, take no runs, and keep history. Duplication
/// copies configuration only (agents hold no secrets, but the rule stands).
/// </summary>
public sealed class AiAgent : AggregateRoot<Guid>
{
    public Guid DepartmentId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Instructions { get; private set; } = default!;
    public string SkillsJson { get; private set; } = "[]";
    public string RuntimeBinding { get; private set; } = default!;
    public string Model { get; private set; } = default!;
    public AiVariant Variant { get; private set; }
    public AgentAccessMode AccessMode { get; private set; }
    public string AccessUserIdsJson { get; private set; } = "[]";
    public bool IsArchived { get; private set; }
    public DateTimeOffset? ArchivedOnUtc { get; private set; }

    private AiAgent() { }

    public static AiAgent Create(
        Guid departmentId,
        string name,
        string instructions,
        IReadOnlyList<string> skills,
        string runtimeBinding,
        string model,
        AiVariant variant,
        AgentAccessMode accessMode,
        IReadOnlyList<string> accessUserIds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(instructions);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeBinding);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(skills);
        ArgumentNullException.ThrowIfNull(accessUserIds);

        return new AiAgent
        {
            Id = Guid.CreateVersion7(),
            DepartmentId = departmentId,
            Name = name.Trim(),
            Instructions = instructions.Trim(),
            SkillsJson = JsonSerializer.Serialize(skills),
            RuntimeBinding = runtimeBinding.Trim(),
            Model = model.Trim(),
            Variant = variant,
            AccessMode = accessMode,
            AccessUserIdsJson = JsonSerializer.Serialize(accessUserIds),
        };
    }

    public void Update(
        string name,
        string instructions,
        IReadOnlyList<string> skills,
        string runtimeBinding,
        string model,
        AiVariant variant,
        AgentAccessMode accessMode,
        IReadOnlyList<string> accessUserIds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(instructions);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeBinding);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        Name = name.Trim();
        Instructions = instructions.Trim();
        SkillsJson = JsonSerializer.Serialize(skills ?? []);
        RuntimeBinding = runtimeBinding.Trim();
        Model = model.Trim();
        Variant = variant;
        AccessMode = accessMode;
        AccessUserIdsJson = JsonSerializer.Serialize(accessUserIds ?? []);
    }

    public void Archive()
    {
        IsArchived = true;
        ArchivedOnUtc = DateTimeOffset.UtcNow;
    }

    public void Restore()
    {
        IsArchived = false;
        ArchivedOnUtc = null;
    }

    public AiAgent Duplicate(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AiAgent
        {
            Id = Guid.CreateVersion7(),
            DepartmentId = DepartmentId,
            Name = name.Trim(),
            Instructions = Instructions,
            SkillsJson = SkillsJson,
            RuntimeBinding = RuntimeBinding,
            Model = Model,
            Variant = Variant,
            AccessMode = AccessMode,
            AccessUserIdsJson = AccessUserIdsJson,
        };
    }

    public IReadOnlyList<string> Skills() =>
        JsonSerializer.Deserialize<List<string>>(SkillsJson) ?? [];

    public IReadOnlyList<string> AccessUserIds() =>
        JsonSerializer.Deserialize<List<string>>(AccessUserIdsJson) ?? [];
}
