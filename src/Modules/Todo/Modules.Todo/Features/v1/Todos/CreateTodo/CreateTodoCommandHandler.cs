using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.Todos.CreateTodo;

public sealed class CreateTodoCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTodoCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = Domain.Todo.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            (TodoPriority)command.Priority,
            command.DueDate);

        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            todo.Notes = command.Notes;
        }

        context.Todos.Add(todo);
        await context.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }
}
