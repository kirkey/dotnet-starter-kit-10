using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.CreateTodoTask;

public sealed class CreateTodoTaskCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTodoTaskCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTodoTaskCommand command, CancellationToken cancellationToken)
    {
        var task = TodoTask.Create(
            command.TodoId,
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.SortOrder);

        context.TodoTasks.Add(task);
        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
