using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.GetTodo;

/// <summary>
/// Handles retrieval of a single todo item by ID.
/// 
/// **Purpose:**
/// Processes the GetTodoQuery by:
/// 1. Finding the todo by ID with all related tasks eager-loaded
/// 2. Projecting to the full TodoDto with task counts
/// 3. Throwing InvalidOperationException if todo not found
/// 
/// **Eager Loading:**
/// Includes related tasks to efficiently calculate:
/// - Total task count
/// - Completed task count
/// 
/// **Error Handling:**
/// Throws InvalidOperationException if the todo doesn't exist.
/// This is a 404-level error that should be caught by the endpoint.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database access
/// </summary>
public sealed class GetTodoQueryHandler(TodoDbContext context)
    : IQueryHandler<GetTodoQuery, TodoDto>
{
    /// <summary>
    /// Handles the GetTodoQuery to retrieve a single todo by ID.
    /// </summary>
    /// <param name="query">The query containing the todo ID to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A complete TodoDto with all details including task counts.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the todo with the specified ID is not found.</exception>
    public async ValueTask<TodoDto> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .Include(t => t.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == query.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {query.Id} not found");
        }

        return new TodoDto(
            todo.Id,
            todo.Name,
            todo.Description,
            todo.Notes,
            todo.Status,
            todo.IsActive,
            (int)todo.Priority,
            todo.DueDate,
            todo.IsCompleted,
            todo.CompletedAt,
            todo.Tasks.Count,
            todo.Tasks.Count(t => t.IsCompleted),
            todo.CreatedOnUtc,
            todo.CreatedByUserName);
    }
}
