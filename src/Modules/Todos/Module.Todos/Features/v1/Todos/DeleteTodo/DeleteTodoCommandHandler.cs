using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Exceptions;

namespace FSH.Module.Todos.Features.v1.Todos.DeleteTodo;

/// <summary>
/// Handles the deletion of a todo item.
/// 
/// **Purpose:**
/// Processes the DeleteTodoCommand to permanently remove a todo and all its tasks.
/// 
/// **Exception Handling:**
/// Uses centralized TodoNotFoundException for missing todos.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class DeleteTodoCommandHandler(TodoDbContext context)
    : ICommandHandler<DeleteTodoCommand>
{
    /// <summary>
    /// Handles the DeleteTodoCommand to delete a todo.
    /// </summary>
    /// <param name="command">The command containing the todo ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful deletion.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when the todo is not found.</exception>
    public async ValueTask<Unit> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        context.Todos.Remove(todo);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
