using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.ReopenTodo;

public class ReopenTodoCommandHandler(TodoDbContext dbContext) : ICommandHandler<ReopenTodoCommand>
{
    public async ValueTask<Unit> Handle(ReopenTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException($"Todo with id {command.Id} not found.");

        todo.Reopen();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
