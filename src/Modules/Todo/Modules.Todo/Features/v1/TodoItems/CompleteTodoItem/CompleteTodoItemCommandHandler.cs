using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoItems.CompleteTodoItem;

public class CompleteTodoItemCommandHandler(TodoDbContext context) : ICommandHandler<CompleteTodoItemCommand>
{
    public async ValueTask<Unit> Handle(CompleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems.FindAsync(new object[] { command.Id }, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Todo Item {command.Id} not found");

        item.Status = "Completed";
        item.CompletedDate = DateTimeOffset.UtcNow;
        if (command.ActualHours.HasValue)
        {
            item.ActualHours = command.ActualHours;
        }

        context.TodoItems.Update(item);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
