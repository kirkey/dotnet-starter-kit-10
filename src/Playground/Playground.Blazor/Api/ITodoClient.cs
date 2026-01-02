using FSH.Playground.Blazor.ApiClient;

namespace FSH.Playground.Blazor.Api;

/// <summary>
/// Combined todo client interface
/// </summary>
public interface ITodosClient
{
    // List operations
    Task<TodosPagedResponse> TodoGetAsync(int page, int pageSize, string searchTerm = null, string status = null, int? priority = null, bool? isCompleted = null, CancellationToken cancellationToken = default(CancellationToken));
    Task<Guid> TodoPostAsync(CreateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken));
    Task<TodoDto> TodoGetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<Guid> TodoPutAsync(Guid id, UpdateTodoCommand body, CancellationToken cancellationToken = default(CancellationToken));
    Task TodoDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    // Task operations
    Task<ICollection<TodoTaskDto>> TasksGetAsync(Guid todoId, CancellationToken cancellationToken = default(CancellationToken));
    Task<Guid> TasksPostAsync(Guid todoId, CreateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken));
    Task<Guid> TasksPutAsync(Guid id, UpdateTodoTaskCommand body, CancellationToken cancellationToken = default(CancellationToken));
    Task TasksDeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task CompleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
