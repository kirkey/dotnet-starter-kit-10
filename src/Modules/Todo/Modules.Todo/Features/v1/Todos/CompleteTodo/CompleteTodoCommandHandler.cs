using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Exceptions;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.Todos.CompleteTodo;

/// <summary>
/// Handles the completion or reopening of a todo item.
/// 
/// **Purpose:**
/// Processes the CompleteTodoCommand by toggling the completion status:
/// - If todo is not completed, marks it as completed
/// - If todo is already completed, reopens it
/// 
/// **Exception Handling:**
/// Uses centralized TodoNotFoundException for missing todos.
/// Ensures consistent exception handling across all handlers.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class CompleteTodoCommandHandler(TodoDbContext context)
    : ICommandHandler<CompleteTodoCommand>
{
    /// <summary>
    /// Handles the CompleteTodoCommand to toggle todo completion status.
    /// </summary>
    /// <param name="command">The command containing the todo ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful completion.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when the todo is not found.</exception>
    public async ValueTask<Unit> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        if (todo.IsCompleted)
        {
            todo.Reopen();
        }
        else
        {
            todo.Complete();
        }

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
