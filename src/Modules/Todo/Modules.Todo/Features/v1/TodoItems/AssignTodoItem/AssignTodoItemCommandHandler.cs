using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoItems.AssignTodoItem;

public class AssignTodoItemCommandHandler(TodoDbContext context) : ICommandHandler<AssignTodoItemCommand>
{
    public async ValueTask<Unit> Handle(AssignTodoItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems.FindAsync(new object[] { command.Id }, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Todo Item {command.Id} not found");

        item.AssignedToUserId = command.UserId;
        item.AssignedToUserName = command.UserName;

        context.TodoItems.Update(item);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
