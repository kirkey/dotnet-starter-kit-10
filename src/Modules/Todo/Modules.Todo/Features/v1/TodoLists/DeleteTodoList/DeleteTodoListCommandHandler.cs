using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoLists.DeleteTodoList;

public class DeleteTodoListCommandHandler(TodoDbContext context) : ICommandHandler<DeleteTodoListCommand>
{
    public async ValueTask<Unit> Handle(DeleteTodoListCommand command, CancellationToken cancellationToken)
    {
        var list = await context.TodoLists.FindAsync(new object[] { command.Id }, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Todo List {command.Id} not found");

        context.TodoLists.Remove(list);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
