using FSH.Playground.Blazor.ApiClient;

namespace FSH.Playground.Blazor.Api;

/// <summary>
/// Todo Client adapter - provides unified interface for all todo operations
/// </summary>
public class TodosClient(ITodosClient todoClient) : ITodosClient
{
    // Delegate to V1Client for list operations
    public Task<TodosPagedResponse> TodoGetAsync(int page, int pageSize, string searchTerm = null, string status = null, int? priority = null, bool? isCompleted = null, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TodoGetAsync(page, pageSize, searchTerm, status, priority, isCompleted, cancellationToken);

    public Task<Guid> TodoPostAsync(CreateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TodoPostAsync(body, cancellationToken);

    public Task<TodoDto> TodoGetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TodoGetAsync(id, cancellationToken);

    public Task<Guid> TodoPutAsync(Guid id, UpdateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TodoPutAsync(id, body, cancellationToken);

    public Task TodoDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TodoDeleteAsync(id, cancellationToken);

    // Delegate to TodoClient for task operations
    public Task<ICollection<TodoTaskDto>> TasksGetAsync(Guid todoId, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksGetAsync(todoId, cancellationToken);

    public Task<Guid> TasksPostAsync(Guid todoId, CreateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksPostAsync(todoId, body, cancellationToken);

    public Task<Guid> TasksPutAsync(Guid id, UpdateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksPutAsync(id, body, cancellationToken);

    public Task TasksDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.TasksDeleteAsync(id, cancellationToken);

    public Task CompleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        => todoClient.CompleteAsync(id, cancellationToken);
}
