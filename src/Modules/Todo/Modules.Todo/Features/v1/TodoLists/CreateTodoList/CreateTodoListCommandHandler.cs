using FSH.Framework.Core.Context;
using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;

namespace FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;

public class CreateTodoListCommandHandler : ICommandHandler<CreateTodoListCommand, Guid>
{
    private readonly TodoDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CreateTodoListCommandHandler(TodoDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async ValueTask<Guid> Handle(CreateTodoListCommand command, CancellationToken ct)
    {
        var todoList = TodoList.Create(
            command.Name,
            _currentUser.GetTenant()!,
            _currentUser.GetUserId(),
            _currentUser.Name,
            command.Description,
            command.Color);

        if (!string.IsNullOrEmpty(command.Notes))
        {
            todoList.Notes = command.Notes;
        }

        _db.TodoLists.Add(todoList);
        await _db.SaveChangesAsync(ct);

        return todoList.Id;
    }
}
