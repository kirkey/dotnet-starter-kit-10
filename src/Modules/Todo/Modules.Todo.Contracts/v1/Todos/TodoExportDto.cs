namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// DTO for exporting todos
/// </summary>
public record TodoExportDto(
    string Name,
    string? Description,
    string? Notes,
    string Status,
    int Priority,
    DateTimeOffset? DueDate,
    bool IsCompleted,
    DateTimeOffset? CompletedAt);
