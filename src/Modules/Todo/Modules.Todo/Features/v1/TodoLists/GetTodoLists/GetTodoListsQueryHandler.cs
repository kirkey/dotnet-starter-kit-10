using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoLists.GetTodoLists;

public class GetTodoListsQueryHandler(TodoDbContext db) : IQueryHandler<GetTodoListsQuery, TodoListsResponse>
{
    public async ValueTask<TodoListsResponse> Handle(GetTodoListsQuery query, CancellationToken ct)
    {
        var baseQuery = db.TodoLists.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.Where(x => x.Name.Contains(query.SearchTerm) || (x.Description != null && x.Description.Contains(query.SearchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            baseQuery = baseQuery.Where(x => x.Status == query.Status);
        }

        if (query.IsActive.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.IsActive == query.IsActive.Value);
        }

        var totalCount = await baseQuery.CountAsync(ct);

        var data = await baseQuery
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new TodoListSummaryDto(
                x.Id,
                x.Name,
                x.Description,
                x.Status,
                x.IsActive,
                x.Color,
                x.Items.Count,
                x.Items.Count(i => i.Status == "Completed"),
                x.CreatedOnUtc,
                x.CreatedByUserName))
            .ToListAsync(ct);

        return new TodoListsResponse(data, totalCount, query.Page, query.PageSize);
    }
}
