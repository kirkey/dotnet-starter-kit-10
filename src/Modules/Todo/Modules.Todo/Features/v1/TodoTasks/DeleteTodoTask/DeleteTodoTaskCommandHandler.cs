using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Exceptions;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.DeleteTodoTask;

/// <summary>
/// Handles the deletion of a todo task.
/// 
/// **Purpose:**
/// Processes the DeleteTodoTaskCommand to permanently remove a task from a todo.
/// 
/// **Exception Handling:**
/// Uses centralized TodoTaskNotFoundException for missing tasks.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class DeleteTodoTaskCommandHandler(TodoDbContext context)
    : ICommandHandler<DeleteTodoTaskCommand>
{
    /// <summary>
    /// Handles the DeleteTodoTaskCommand to delete a task.
    /// </summary>
    /// <param name="command">The command containing the task ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful deletion.</returns>
    /// <exception cref="TodoTaskNotFoundException">Thrown when the task is not found.</exception>
    public async ValueTask<Unit> Handle(DeleteTodoTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await context.TodoTasks
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        context.TodoTasks.Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
