using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoLists.UpdateTodoList;

public class UpdateTodoListCommandHandler(TodoDbContext db, ICurrentUser currentUser)
    : ICommandHandler<UpdateTodoListCommand, bool>
{
    public async ValueTask<bool> Handle(UpdateTodoListCommand command, CancellationToken ct)
    {
        var todoList = await db.TodoLists.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (todoList is null)
        {
            return false;
        }

        todoList.Update(
            command.Name,
            currentUser.GetUserId(),
            currentUser.Name,
            command.Description,
            command.Color,
            command.Status,
            command.IsActive);

        await db.SaveChangesAsync(ct);
        return true;
    }
}
