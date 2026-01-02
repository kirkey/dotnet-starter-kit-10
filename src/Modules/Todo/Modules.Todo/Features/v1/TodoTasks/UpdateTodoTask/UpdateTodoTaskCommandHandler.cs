using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.UpdateTodoTask;

public sealed class UpdateTodoTaskCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTodoTaskCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateTodoTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException($"Task with ID {command.Id} not found");
        }

        task.Update(
            command.Name,
            command.Description,
            command.SortOrder,
            currentUser.GetUserId(),
            currentUser.Name ?? "System");

        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
