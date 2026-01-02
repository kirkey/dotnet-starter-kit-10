using FSH.Framework.Persistence;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItem;

public class GetTodoItemQueryHandler(TodoDbContext context) : IQueryHandler<GetTodoItemQuery, TodoItemResponse?>
{
    public async ValueTask<TodoItemResponse?> Handle(GetTodoItemQuery query, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems
            .Where(x => x.Id == query.Id)
            .Select(x => new TodoItemResponse(
                x.Id,
                x.TodoListId,
                x.Name,
                x.Description,
                x.Notes,
                x.Status,
                x.IsActive,
                (int)x.Priority,
                x.SortOrder,
                x.DueDate,
                x.CompletedDate,
                x.EstimatedHours,
                x.ActualHours,
                x.AssignedToUserId,
                x.AssignedToUserName,
                x.CreatedOnUtc,
                x.CreatedBy,
                x.CreatedByUserName,
                x.LastModifiedOnUtc,
                x.LastModifiedBy,
                x.LastModifiedByUserName))
            .FirstOrDefaultAsync(cancellationToken);

        return item;
    }
}
