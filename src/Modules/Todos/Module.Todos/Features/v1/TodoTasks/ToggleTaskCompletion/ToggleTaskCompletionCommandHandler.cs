using FSH.Module.Todos.Contracts.v1.TodoTasks;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Exceptions;

namespace FSH.Module.Todos.Features.v1.TodoTasks.ToggleTaskCompletion;

/// <summary>
/// Handles toggling the completion status of a todo task.
/// 
/// **Purpose:**
/// Processes the ToggleTaskCompletionCommand to flip task completion status:
/// - If completed, marks as pending
/// - If pending, marks as completed
/// 
/// **Exception Handling:**
/// Uses centralized TodoTaskNotFoundException for missing tasks.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class ToggleTaskCompletionCommandHandler(TodoDbContext context)
    : ICommandHandler<ToggleTaskCompletionCommand>
{
    /// <summary>
    /// Handles the ToggleTaskCompletionCommand to toggle task completion.
    /// </summary>
    /// <param name="command">The command containing the task ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful toggle.</returns>
    /// <exception cref="TodoTaskNotFoundException">Thrown when the task is not found.</exception>
    public async ValueTask<Unit> Handle(ToggleTaskCompletionCommand command, CancellationToken cancellationToken)
    {
        var task = await context.TodoTasks
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        task.ToggleCompletion();

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
