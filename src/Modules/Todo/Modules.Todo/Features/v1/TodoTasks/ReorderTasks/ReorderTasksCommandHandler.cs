using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.ReorderTasks;

public class ReorderTasksCommandHandler(TodoDbContext dbContext) : ICommandHandler<ReorderTasksCommand>
{
    public async ValueTask<Unit> Handle(ReorderTasksCommand command, CancellationToken cancellationToken)
    {
        var tasks = await dbContext.TodoTasks
            .Where(t => t.TodoId == command.TodoId)
            .ToListAsync(cancellationToken);

        if (!tasks.Any())
        {
            throw new NotFoundException($"No tasks found for todo {command.TodoId}");
        }

        foreach (var orderItem in command.Tasks)
        {
            var task = tasks.FirstOrDefault(t => t.Id == orderItem.TaskId);
            if (task != null)
            {
                typeof(Domain.TodoTask)
                    .GetProperty(nameof(Domain.TodoTask.SortOrder))!
                    .SetValue(task, orderItem.SortOrder);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
