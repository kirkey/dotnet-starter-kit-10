using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoLists.UpdateTodoList;

public class UpdateTodoListCommandHandler : ICommandHandler<UpdateTodoListCommand, bool>
{
    private readonly TodoDbContext _db;
    private readonly ICurrentUser _currentUser;

    public UpdateTodoListCommandHandler(TodoDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async ValueTask<bool> Handle(UpdateTodoListCommand command, CancellationToken ct)
    {
        var todoList = await _db.TodoLists.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (todoList is null)
        {
            return false;
        }

        todoList.Update(
            command.Name,
            _currentUser.GetUserId(),
            _currentUser.Name,
            command.Description,
            command.Color,
            command.Status,
            command.IsActive);

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
