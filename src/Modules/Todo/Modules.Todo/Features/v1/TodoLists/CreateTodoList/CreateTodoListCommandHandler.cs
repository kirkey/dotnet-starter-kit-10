using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;

public class CreateTodoListCommandHandler(TodoDbContext db, ICurrentUser currentUser)
    : ICommandHandler<CreateTodoListCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTodoListCommand command, CancellationToken ct)
    {
        var todoList = TodoList.Create(
            command.Name,
            currentUser.GetTenant()!,
            currentUser.GetUserId(),
            currentUser.Name,
            command.Description,
            command.Color);

        if (!string.IsNullOrEmpty(command.Notes))
        {
            todoList.Notes = command.Notes;
        }

        db.TodoLists.Add(todoList);
        await db.SaveChangesAsync(ct);

        return todoList.Id;
    }
}
