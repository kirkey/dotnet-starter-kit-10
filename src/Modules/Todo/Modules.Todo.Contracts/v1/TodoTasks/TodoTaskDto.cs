namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record TodoTaskDto(
    Guid Id,
    Guid TodoId,
    string Name,
    string? Description,
    string Status,
    bool IsCompleted,
    DateTimeOffset? CompletedAt,
    int SortOrder,
    DateTimeOffset CreatedOnUtc);
