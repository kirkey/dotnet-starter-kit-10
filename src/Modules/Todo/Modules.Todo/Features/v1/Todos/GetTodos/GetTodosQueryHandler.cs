using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.GetTodos;

public sealed class GetTodosQueryHandler(TodoDbContext context)
    : IQueryHandler<GetTodosQuery, TodosPagedResponse>
{
    public async ValueTask<TodosPagedResponse> Handle(GetTodosQuery query, CancellationToken cancellationToken)
    {
        var todosQuery = context.Todos
            .Include(t => t.Tasks)
            .AsNoTracking()
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            todosQuery = todosQuery.Where(t =>
                t.Name.Contains(query.SearchTerm) ||
                (t.Description != null && t.Description.Contains(query.SearchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            todosQuery = todosQuery.Where(t => t.Status == query.Status);
        }

        if (query.Priority.HasValue)
        {
            todosQuery = todosQuery.Where(t => (int)t.Priority == query.Priority.Value);
        }

        if (query.IsCompleted.HasValue)
        {
            todosQuery = todosQuery.Where(t => t.IsCompleted == query.IsCompleted.Value);
        }

        // Order by due date and creation date
        todosQuery = todosQuery.OrderBy(t => t.DueDate).ThenByDescending(t => t.CreatedOnUtc);

        // Get total count
        var totalCount = await todosQuery.CountAsync(cancellationToken);

        // Apply pagination and project to DTO
        var data = await todosQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => new TodoSummaryDto(
                t.Id,
                t.Name,
                t.Description,
                t.Status,
                (int)t.Priority,
                t.DueDate,
                t.IsCompleted,
                t.Tasks.Count,
                t.Tasks.Count(task => task.IsCompleted)))
            .ToListAsync(cancellationToken);

        return new TodosPagedResponse(data, totalCount, query.Page, query.PageSize);
    }
}
