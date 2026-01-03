using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.UpdateTodo;

/// <summary>
/// Handles the update of an existing todo item.
/// 
/// **Purpose:**
/// Processes the UpdateTodoCommand by:
/// 1. Finding the existing todo by ID
/// 2. Updating its properties via the domain aggregate's Update method
/// 3. Updating the Notes property separately
/// 4. Recording modification audit trail
/// 5. Persisting changes to database
/// 
/// **Domain Logic:**
/// Uses the Todo.Update() method to ensure:
/// - All properties are validated
/// - Priority enum conversion is handled correctly
/// - DueDate is normalized to UTC
/// - Modification tracking is updated
/// 
/// **Error Handling:**
/// Throws InvalidOperationException if the todo doesn't exist (404).
/// 
/// **Dependencies:**
/// - TodoDbContext: For database access and persistence
/// - ICurrentUser: For accessing current user context (ID, username)
/// </summary>
public sealed class UpdateTodoCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTodoCommand, Guid>
{
    /// <summary>
    /// Handles the UpdateTodoCommand to update an existing todo.
    /// </summary>
    /// <param name="command">The command containing the todo ID and updated properties.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The ID of the updated todo.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the todo with the specified ID is not found.</exception>
    public async ValueTask<Guid> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {command.Id} not found");
        }

        todo.Update(
            command.Name,
            command.Description,
            (TodoPriority)command.Priority,
            command.DueDate,
            currentUser.GetUserId(),
            currentUser.Name ?? "System");

        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            todo.Notes = command.Notes;
        }

        await context.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }
}
