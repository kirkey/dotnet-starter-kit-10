using FSH.Framework.Core.Context;
using FSH.Modules.Todos.Contracts.v1.Todos;
using FSH.Modules.Todos.Data;
using FSH.Modules.Todos.Domain;

namespace FSH.Modules.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Handles the creation of a new todo item.
/// 
/// **Purpose:**
/// Processes the CreateTodoCommand by:
/// 1. Creating a new Todo aggregate from the command data
/// 2. Setting additional todo properties (Notes)
/// 3. Persisting to the database
/// 4. Returning the new todo's ID
/// 
/// **Domain Logic:**
/// - Uses the Todo.Create factory method to ensure proper initialization
/// - Converts priority enum value from command
/// - Associates todo with the current user and tenant
/// - Maintains audit trail through creation tracking
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// - ICurrentUser: For accessing current user context (ID, username, tenant)
/// </summary>
public sealed class CreateTodoCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTodoCommand, Guid>
{
    /// <summary>
    /// Handles the CreateTodoCommand to create a new todo item.
    /// </summary>
    /// <param name="command">The command containing todo creation details.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The ID of the newly created todo.</returns>
    public async ValueTask<Guid> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = Domain.Todo.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            (TodoPriority)command.Priority,
            command.DueDate);

        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            todo.Notes = command.Notes;
        }

        context.Todos.Add(todo);
        await context.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }
}
