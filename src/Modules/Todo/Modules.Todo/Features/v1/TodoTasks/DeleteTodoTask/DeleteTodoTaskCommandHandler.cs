using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.DeleteTodoTask;

public sealed class DeleteTodoTaskCommandHandler(TodoDbContext context)
    : ICommandHandler<DeleteTodoTaskCommand>
{
    public async ValueTask<Unit> Handle(DeleteTodoTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException($"Task with ID {command.Id} not found");
        }

        context.TodoTasks.Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
