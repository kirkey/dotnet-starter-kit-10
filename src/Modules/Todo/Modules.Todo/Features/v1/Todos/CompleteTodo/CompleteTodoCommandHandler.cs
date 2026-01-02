using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.CompleteTodo;

public sealed class CompleteTodoCommandHandler(TodoDbContext context)
    : ICommandHandler<CompleteTodoCommand>
{
    public async ValueTask<Unit> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {command.Id} not found");
        }

        if (todo.IsCompleted)
        {
            todo.Reopen();
        }
        else
        {
            todo.Complete();
        }

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
