using FSH.Modules.Todo.Contracts.v1.TodoLists;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoLists.GetTodoList;

public class GetTodoListQueryHandler(TodoDbContext db) : IQueryHandler<GetTodoListQuery, TodoListResponse?>
{
    public async ValueTask<TodoListResponse?> Handle(GetTodoListQuery query, CancellationToken ct)
    {
        return await db.TodoLists
            .Where(l => l.Id == query.Id)
            .Select(l => new TodoListResponse(
                l.Id,
                l.Name,
                l.Description,
                l.Notes,
                l.Status,
                l.IsActive,
                l.Color,
                l.SortOrder,
                l.DueDate,
                l.CreatedOnUtc,
                l.CreatedBy,
                l.CreatedByUserName,
                l.LastModifiedOnUtc,
                l.LastModifiedBy,
                l.LastModifiedByUserName,
                l.Items.Select(i => new TodoItemDto(
                    i.Id,
                    i.Name,
                    i.Description,
                    i.Status,
                    (int)i.Priority,
                    i.DueDate,
                    i.CompletedDate,
                    i.AssignedToUserName
                )).ToList()
            ))
            .FirstOrDefaultAsync(ct);
    }
}
