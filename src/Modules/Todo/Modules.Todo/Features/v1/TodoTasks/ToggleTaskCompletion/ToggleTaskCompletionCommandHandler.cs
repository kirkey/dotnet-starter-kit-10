using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.ToggleTaskCompletion;

public sealed class ToggleTaskCompletionCommandHandler(TodoDbContext context)
    : ICommandHandler<ToggleTaskCompletionCommand>
{
    public async ValueTask<Unit> Handle(ToggleTaskCompletionCommand command, CancellationToken cancellationToken)
    {
        var task = await context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException($"Task with ID {command.Id} not found");
        }

        task.ToggleCompletion();

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
