using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Exceptions;

namespace FSH.Module.Todos.Features.v1.Todos.ArchiveTodo;

/// <summary>
/// Handles the archival of a todo item (soft delete).
/// 
/// **Purpose:**
/// Processes the ArchiveTodoCommand to mark a todo as archived without deletion.
/// 
/// **Exception Handling:**
/// Uses centralized TodoNotFoundException for missing todos.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class ArchiveTodoCommandHandler(TodoDbContext dbContext)
    : ICommandHandler<ArchiveTodoCommand>
{
    /// <summary>
    /// Handles the ArchiveTodoCommand to archive a todo.
    /// </summary>
    /// <param name="command">The command containing the todo ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful archival.</returns>
    /// <exception cref="TodoNotFoundException">Thrown when the todo is not found.</exception>
    public async ValueTask<Unit> Handle(ArchiveTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Todos
            .Where(t => t.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, cancellationToken);

        todo.Archive();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
