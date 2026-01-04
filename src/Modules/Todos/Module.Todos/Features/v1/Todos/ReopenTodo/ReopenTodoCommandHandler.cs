using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Exceptions;

namespace FSH.Module.Todos.Features.v1.Todos.ReopenTodo;

/// <summary>
/// Handles the reopening of a completed todo.
/// 
/// **Purpose:**
/// Processes the ReopenTodoCommand to mark a completed todo as reopened.
/// 
/// **Exception Handling:**
/// Uses centralized TodoNotFoundException for missing todos.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class ReopenTodoCommandHandler(TodoDbContext dbContext)
    : ICommandHandler<ReopenTodoCommand>
{
    /// <summary>
    /// Handles the ReopenTodoCommand to reopen a completed todo.
    /// </summary>
    /// <param name="command">The command containing the todo ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful reopening.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when the todo is not found.</exception>
    public async ValueTask<Unit> Handle(ReopenTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Todos
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        todo.Reopen();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
