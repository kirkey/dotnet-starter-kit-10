using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.ReorderTasks;

/// <summary>
/// Handles reordering of todo tasks within a todo item.
/// 
/// **Purpose:**
/// Processes the ReorderTasksCommand to update the sort order of multiple tasks.
/// Useful for drag-and-drop reordering UI operations.
/// 
/// **Exception Handling:**
/// - NotFoundException: Parent Todo doesn't exist or no tasks found
/// 
/// **Domain Logic:**
/// Uses TodoTask.UpdateSortOrder() domain method to properly update each task.
/// Ensures audit trail is maintained for all modifications.
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// </summary>
public sealed class ReorderTasksCommandHandler(TodoDbContext dbContext)
    : ICommandHandler<ReorderTasksCommand>
{
    /// <summary>
    /// Handles the ReorderTasksCommand to reorder tasks within a todo.
    /// </summary>
    /// <param name="command">The command containing parent todo ID and task order list.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Unit (void) on successful reordering.</returns>
    /// <exception cref="NotFoundException">Thrown when no tasks are found for the todo.</exception>
    public async ValueTask<Unit> Handle(ReorderTasksCommand command, CancellationToken cancellationToken)
    {
        var tasks = await dbContext.TodoTasks
            .Where(t => t.TodoId == command.TodoId)
            .ToListAsync(cancellationToken);

        if (!tasks.Any())
        {
            throw new NotFoundException($"No tasks found for todo {command.TodoId}");
        }

        // Update sort order for each task using domain method
        foreach (var orderItem in command.Tasks)
        {
            var task = tasks.FirstOrDefault(t => t.Id == orderItem.TaskId);
            if (task != null)
            {
                task.UpdateSortOrder(orderItem.SortOrder);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
