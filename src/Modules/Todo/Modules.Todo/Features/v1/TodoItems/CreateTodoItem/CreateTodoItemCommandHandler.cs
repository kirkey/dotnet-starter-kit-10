using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoItems.CreateTodoItem;

public class CreateTodoItemCommandHandler(TodoDbContext db, ICurrentUser currentUser)
    : ICommandHandler<CreateTodoItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTodoItemCommand command, CancellationToken ct)
    {
        // Verify the TodoList exists
        var listExists = await db.TodoLists.AnyAsync(l => l.Id == command.TodoListId, ct);
        if (!listExists)
        {
            throw new InvalidOperationException($"TodoList with ID {command.TodoListId} not found");
        }

        var todoItem = TodoItem.Create(
            command.Name,
            command.TodoListId,
            currentUser.GetUserId(),
            currentUser.Name,
            command.Description,
            (TodoPriority)command.Priority);

        if (!string.IsNullOrEmpty(command.Notes))
        {
            todoItem.Notes = command.Notes;
        }

        if (command.DueDate.HasValue)
        {
            todoItem.DueDate = command.DueDate;
        }

        if (command.EstimatedHours.HasValue)
        {
            todoItem.EstimatedHours = command.EstimatedHours;
        }

        db.TodoItems.Add(todoItem);
        await db.SaveChangesAsync(ct);

        return todoItem.Id;
    }
}
