using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Todos.Exceptions;

/// <summary>
/// Extension methods for common exception throwing patterns.
/// 
/// **Purpose:**
/// Provides fluent, reusable methods for validating entity existence and throwing
/// appropriate exceptions. This reduces code duplication and ensures consistent
/// exception handling across all handlers.
/// 
/// **Benefits:**
/// - Single point of exception definition
/// - Reduced code duplication
/// - Consistent error messages
/// - Fluent API for validation
/// - Easier testing and maintenance
/// 
/// **Usage Examples:**
/// var todo = await context.Todos.GetByIdOrThrowAsync(id, ct);
/// var task = await context.TodoTasks.GetByIdOrThrowAsync(id, ct);
/// </summary>
public static class TodoExceptionExtensions
{
    /// <summary>
    /// Gets a Todo by ID or throws TodoNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var todo = await context.Todos
    ///     .Where(t => t.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found Todo or throws exception.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when Todo is not found.</exception>
    public static async Task<Domain.Todo> GetByIdOrThrowAsync(
        this IQueryable<Domain.Todo> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new TodoNotFoundException(id);
    }

    /// <summary>
    /// Gets a TodoTask by ID or throws TodoTaskNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var task = await context.TodoTasks
    ///     .Where(t => t.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found TodoTask or throws exception.</returns>
    /// <exception cref="TodoTaskNotFoundException">Thrown when TodoTask is not found.</exception>
    public static async Task<Domain.TodoTask> GetByIdOrThrowAsync(
        this IQueryable<Domain.TodoTask> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new TodoTaskNotFoundException(id);
    }

    /// <summary>
    /// Checks if a Todo exists by ID or throws TodoNotFoundException if not found.
    /// 
    /// Useful when you need to validate parent existence without loading the full entity.
    /// More efficient than GetByIdOrThrowAsync when entity data is not needed.
    /// 
    /// **Usage:**
    /// await context.Todos.EnsureExistsByIdAsync(command.TodoId, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ParentTodoNotFoundException">Thrown when Todo is not found.</exception>
    public static async Task EnsureExistsByIdAsync(
        this IQueryable<Domain.Todo> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await query.AnyAsync(cancellationToken);
        if (!exists)
        {
            throw new ParentTodoNotFoundException(id);
        }
    }

    /// <summary>
    /// Gets a Todo by ID synchronously or throws TodoNotFoundException if not found.
    /// 
    /// Use this only when you have a list already loaded in memory.
    /// For database queries, use GetByIdOrThrowAsync instead.
    /// 
    /// **Usage:**
    /// var todo = todos.FirstOrDefault(t => t.Id == id)
    ///     ?? throw new TodoNotFoundException(id);
    /// </summary>
    /// <param name="todos">The enumerable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <returns>The found Todo or throws exception.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when Todo is not found.</exception>
    public static Domain.Todo GetByIdOrThrow(
        this IEnumerable<Domain.Todo> todos,
        Guid id)
    {
        return todos.FirstOrDefault(t => t.Id == id)
            ?? throw new TodoNotFoundException(id);
    }

    /// <summary>
    /// Gets a TodoTask by ID synchronously or throws TodoTaskNotFoundException if not found.
    /// 
    /// Use this only when you have a list already loaded in memory.
    /// For database queries, use GetByIdOrThrowAsync instead.
    /// 
    /// **Usage:**
    /// var task = tasks.FirstOrDefault(t => t.Id == id)
    ///     ?? throw new TodoTaskNotFoundException(id);
    /// </summary>
    /// <param name="tasks">The enumerable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <returns>The found TodoTask or throws exception.</returns>
    /// <exception cref="TodoTaskNotFoundException">Thrown when TodoTask is not found.</exception>
    public static Domain.TodoTask GetByIdOrThrow(
        this IEnumerable<Domain.TodoTask> tasks,
        Guid id)
    {
        return tasks.FirstOrDefault(t => t.Id == id)
            ?? throw new TodoTaskNotFoundException(id);
    }
}
