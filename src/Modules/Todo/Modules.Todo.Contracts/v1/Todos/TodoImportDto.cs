namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// DTO for importing todos
/// </summary>
public class TodoImportDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public string Status { get; init; } = "NotStarted";
    public int Priority { get; init; }
    public DateTimeOffset? DueDate { get; init; }
}
