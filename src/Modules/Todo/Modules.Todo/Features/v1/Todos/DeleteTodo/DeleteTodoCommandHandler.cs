using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.DeleteTodo;

public sealed class DeleteTodoCommandHandler(TodoDbContext context)
    : ICommandHandler<DeleteTodoCommand>
{
    public async ValueTask<Unit> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {command.Id} not found");
        }

        context.Todos.Remove(todo);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
