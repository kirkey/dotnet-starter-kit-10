using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Todos.Features.v1.Todos.GetTodos;

/// <summary>
/// Handles the retrieval of paginated todos with filtering and sorting.
/// 
/// **Purpose:**
/// Processes the GetTodosQuery by:
/// 1. Building a queryable set of todos with task relationships
/// 2. Applying optional filters (search term, status, priority, completion)
/// 3. Sorting by due date and creation date
/// 4. Implementing pagination
/// 5. Projecting to TodoSummaryDto
/// 6. Returning paginated response with total count
/// 
/// **Filtering:**
/// - SearchTerm: Case-sensitive contains search on Name or Description
/// - Status: Exact match on status (e.g., "NotStarted", "InProgress", "Completed")
/// - Priority: Filters by priority level
/// - IsCompleted: Filters by completion status
/// 
/// **Sorting:**
/// - Primary: By DueDate (earliest first)
/// - Secondary: By CreatedOnUtc (newest first)
/// 
/// **Performance:**
/// - Eager loads Tasks to count completed tasks
/// - Uses AsNoTracking for read-only query
/// - Projects to DTO before applying pagination
/// 
/// **Dependencies:**
/// - TodoDbContext: For database access
/// </summary>
public sealed class GetTodosQueryHandler(TodoDbContext context)
    : IQueryHandler<GetTodosQuery, TodosPagedResponse>
{
    /// <summary>
    /// Handles the GetTodosQuery to retrieve a paginated list of todos.
    /// </summary>
    /// <param name="query">The query containing pagination and filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A TodosPagedResponse containing the requested page of todos and metadata.</returns>
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
