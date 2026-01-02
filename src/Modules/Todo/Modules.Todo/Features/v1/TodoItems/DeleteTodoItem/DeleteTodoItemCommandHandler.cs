using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoItems.DeleteTodoItem;

public class DeleteTodoItemCommandHandler(TodoDbContext context) : ICommandHandler<DeleteTodoItemCommand>
{
    public async ValueTask<Unit> Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems.FindAsync(new object[] { command.Id }, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Todo Item {command.Id} not found");

        context.TodoItems.Remove(item);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
