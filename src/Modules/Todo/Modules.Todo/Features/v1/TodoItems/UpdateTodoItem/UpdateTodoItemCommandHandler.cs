using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoItems.UpdateTodoItem;

public class UpdateTodoItemCommandHandler(TodoDbContext context) : ICommandHandler<UpdateTodoItemCommand>
{
    public async ValueTask<Unit> Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems.FindAsync(new object[] { command.Id }, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Todo Item {command.Id} not found");

        item.Name = command.Name;
        item.Description = command.Description;
        item.Notes = command.Notes;
        if (command.Status is not null) item.Status = command.Status;
        if (command.IsActive.HasValue) item.IsActive = command.IsActive.Value;
        if (command.Priority.HasValue) item.Priority = (Domain.TodoPriority)command.Priority.Value;
        item.DueDate = command.DueDate;
        item.EstimatedHours = command.EstimatedHours;
        item.AssignedToUserId = command.AssignedToUserId;
        item.AssignedToUserName = command.AssignedToUserName;

        context.TodoItems.Update(item);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
