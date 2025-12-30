using System.Net.Http.Json;
using FSH.Playground.Blazor.ApiClient;

namespace FSH.Playground.Blazor.Services;

public interface ITodoClient
{
    Task<TodoListsResponse?> GetTodoListsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<TodoListDetailResponse?> GetTodoListAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateTodoListAsync(CreateTodoListRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateTodoListAsync(Guid id, UpdateTodoListRequest request, CancellationToken cancellationToken = default);
    Task DeleteTodoListAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateTodoItemAsync(CreateTodoItemRequest request, CancellationToken cancellationToken = default);
}

public class TodoClient : ITodoClient
{
    private readonly HttpClient _httpClient;

    public TodoClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TodoListsResponse?> GetTodoListsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/v1/todo/lists?pageNumber={pageNumber}&pageSize={pageSize}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoListsResponse>(cancellationToken);
    }

    public async Task<TodoListDetailResponse?> GetTodoListAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/v1/todo/lists/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoListDetailResponse>(cancellationToken);
    }

    public async Task<Guid> CreateTodoListAsync(CreateTodoListRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/todo/lists", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
    }

    public async Task<bool> UpdateTodoListAsync(Guid id, UpdateTodoListRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/v1/todo/lists/{id}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
    }

    public async Task DeleteTodoListAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/todo/lists/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Guid> CreateTodoItemAsync(CreateTodoItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/todo/items", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
    }
}

// DTOs
public record TodoListsResponse(List<TodoListSummaryDto> Data, int TotalCount);
public record TodoListDetailResponse(Guid Id, string Name, string? Description, string Status, bool IsActive, string? Color, List<TodoItemDto> Items);
public record CreateTodoListRequest(string Name, string? Description, string? Color);
public record UpdateTodoListRequest(string Name, string? Description, string? Color, bool IsActive);
public record CreateTodoItemRequest(Guid TodoListId, string Name, string? Description, int Priority);

public record TodoListSummaryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string? Color { get; init; }
    public int ItemCount { get; init; }
    public int CompletedItemCount { get; init; }
    public DateTimeOffset CreatedOnUtc { get; init; }
    public string? CreatedByUserName { get; init; }
}

public record TodoItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Status { get; init; } = string.Empty;
    public int Priority { get; init; }
    public string? AssignedToUserName { get; init; }
    public DateTimeOffset? DueDate { get; init; }
    public DateTimeOffset? CompletedDate { get; init; }
}
