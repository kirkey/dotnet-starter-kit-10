namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record TodoDto(
    Guid Id,
    string Name,
    string? Description,
    string? Notes,
    string Status,
    bool IsActive,
    int Priority,
    DateTimeOffset? DueDate,
    bool IsCompleted,
    DateTimeOffset? CompletedAt,
    int TaskCount,
    int CompletedTaskCount,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);

public record TodoSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    int Priority,
    DateTimeOffset? DueDate,
    bool IsCompleted,
    int TaskCount,
    int CompletedTaskCount);
