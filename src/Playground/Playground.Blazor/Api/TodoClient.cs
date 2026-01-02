namespace FSH.Playground.Blazor.Api;

/// <summary>
/// Todo Client adapter - provides unified interface for all todo operations
/// Combines V1Client (CRUD operations), TodoClient (todo actions), and TasksClient (task operations)
/// </summary>
public class TodosClient(IV1Client v1Client, ITodoClient todoClient, ITasksClient tasksClient) : ITodosClient
{
    // Delegate to V1Client for list operations
    public Task<TodosPagedResponse> TodoGetAsync(int page, int pageSize, string searchTerm = null, string status = null, int? priority = null, bool? isCompleted = null, CancellationToken cancellationToken = default(CancellationToken))
        => v1Client.TodoGetAsync(page, pageSize, searchTerm, status, priority, isCompleted, cancellationToken);

    public Task<Guid> TodoPostAsync(CreateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => v1Client.TodoPostAsync(body, cancellationToken);

    public Task<TodoDto> TodoGetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => v1Client.TodoGetAsync(id, cancellationToken);

    public Task<Guid> TodoPutAsync(Guid id, UpdateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => v1Client.TodoPutAsync(id, body, cancellationToken);

    public Task TodoDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => v1Client.TodoDeleteAsync(id, cancellationToken);

    public Task TodoCompleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.CompleteAsync(id, cancellationToken);

    public Task TodoReopenAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.ReopenAsync(id, cancellationToken);

    public Task TodoUpdateStatusAsync(Guid id, int status, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.StatusAsync(id, status, cancellationToken);

    public Task TodoArchiveAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.ArchiveAsync(id, cancellationToken);

    // Delegate to TodoClient for task operations
    public Task<ICollection<TodoTaskDto>> TasksGetAsync(Guid todoId, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksGetAsync(todoId, cancellationToken);

    public Task<Guid> TasksPostAsync(Guid todoId, CreateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksPostAsync(todoId, body, cancellationToken);

    public Task<Guid> TasksPutAsync(Guid id, UpdateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksPutAsync(id, body, cancellationToken);

    public Task TasksDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksDeleteAsync(id, cancellationToken);

    public Task TasksCompleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => tasksClient.ToggleAsync(id, cancellationToken);

    public Task TasksReorderAsync(Guid todoId, IEnumerable<TaskOrderItem> tasks, CancellationToken cancellationToken = default(CancellationToken))
        => tasksClient.ReorderAsync(todoId, tasks, cancellationToken);

    // Import/Export operations
    public Task ExportAsync(string format, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.ExportAsync(format, cancellationToken);

    public Task ImportAsync(FileParameter file, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.ImportAsync(file, cancellationToken);
}
