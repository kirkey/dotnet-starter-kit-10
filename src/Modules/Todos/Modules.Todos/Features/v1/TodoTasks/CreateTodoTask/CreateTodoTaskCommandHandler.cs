using FSH.Framework.Core.Context;
using FSH.Modules.Todos.Contracts.v1.TodoTasks;
using FSH.Modules.Todos.Data;
using FSH.Modules.Todos.Domain;
using FSH.Modules.Todos.Exceptions;

namespace FSH.Modules.Todos.Features.v1.TodoTasks.CreateTodoTask;

/// <summary>
/// Handles the creation of a new task within a todo item.
/// 
/// **Purpose:**
/// Processes the CreateTodoTaskCommand by:
/// 1. Validating parent Todo exists
/// 2. Creating new TodoTask aggregate
/// 3. Persisting to database
/// 4. Recording audit trail
/// 
/// **Domain Logic:**
/// Uses TodoTask.Create factory method with proper initialization.
/// Validates parent Todo existence to maintain data integrity.
/// 
/// **Exception Handling:**
/// - ParentTodoNotFoundException: Parent Todo doesn't exist
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// - ICurrentUser: For user context (ID, username, tenant)
/// </summary>
public sealed class CreateTodoTaskCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTodoTaskCommand, Guid>
{
    /// <summary>
    /// Handles the CreateTodoTaskCommand to create a new task within a todo.
    /// </summary>
    /// <param name="command">The command containing task creation details and parent todo ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The ID of the newly created task.</returns>
    /// <exception cref="ParentTodoNotFoundException">Thrown when parent Todo doesn't exist.</exception>
    public async ValueTask<Guid> Handle(CreateTodoTaskCommand command, CancellationToken cancellationToken)
    {
        // Validate parent Todo exists
        await context.Todos
            .Where(t => t.Id == command.TodoId)
            .EnsureExistsByIdAsync(command.TodoId, cancellationToken);

        var task = TodoTask.Create(
            command.TodoId,
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.SortOrder);

        context.TodoTasks.Add(task);
        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
