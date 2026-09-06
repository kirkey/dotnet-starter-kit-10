using FSH.Framework.Core.Domain;

namespace FSH.Modules.Ai.Domain;

/// <summary>Tenant sub-scope that owns agents — an organizational grouping, not a tenancy layer.</summary>
public sealed class AiDepartment : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private AiDepartment() { }

    public static AiDepartment Create(string name, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AiDepartment
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
        };
    }

    public void Update(string name, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }
}
