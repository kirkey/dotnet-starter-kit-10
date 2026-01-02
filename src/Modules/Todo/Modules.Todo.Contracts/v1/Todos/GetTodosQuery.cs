using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record GetTodosQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? Status = null,
    int? Priority = null,
    bool? IsCompleted = null) : IQuery<TodosPagedResponse>;

public record TodosPagedResponse(
    List<TodoSummaryDto> Data,
    int TotalCount,
    int Page,
    int PageSize);
