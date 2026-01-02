using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.UpdateTodo;

public sealed class UpdateTodoCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTodoCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {command.Id} not found");
        }

        todo.Update(
            command.Name,
            command.Description,
            (TodoPriority)command.Priority,
            command.DueDate,
            currentUser.GetUserId(),
            currentUser.Name ?? "System");

        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            todo.Notes = command.Notes;
        }

        await context.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }
}
