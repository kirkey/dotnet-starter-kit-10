using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.CreateTodoTask;

/// <summary>
/// Handles the creation of a new task within a todo item.
/// 
/// **Purpose:**
/// Processes the CreateTodoTaskCommand by:
/// 1. Creating a new TodoTask aggregate using the factory method
/// 2. Associating it with the parent todo
/// 3. Setting the task's position via sort order
/// 4. Recording creation audit trail with current user context
/// 5. Persisting to the database
/// 
/// **Domain Logic:**
/// Uses the TodoTask.Create factory method to ensure:
/// - All required properties are initialized
/// - Status is set to "Pending"
/// - IsActive flag is set appropriately
/// - Audit trail is recorded
/// - Sort order is assigned for task list positioning
/// 
/// **Multi-Tenancy:**
/// Associates the task with the current user's tenant for isolation.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// - ICurrentUser: For accessing current user context (ID, username, tenant)
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
    public async ValueTask<Guid> Handle(CreateTodoTaskCommand command, CancellationToken cancellationToken)
    {
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
