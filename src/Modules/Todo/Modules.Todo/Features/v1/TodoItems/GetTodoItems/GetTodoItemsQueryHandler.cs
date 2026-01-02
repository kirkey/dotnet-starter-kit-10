using FSH.Framework.Persistence;
using FSH.Modules.Todo.Contracts.v1.TodoItems;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItems;

public class GetTodoItemsQueryHandler(TodoDbContext context) : IQueryHandler<GetTodoItemsQuery, List<TodoItemResponse>>
{
    public async ValueTask<List<TodoItemResponse>> Handle(GetTodoItemsQuery query, CancellationToken cancellationToken)
    {
        var items = await context.TodoItems
            .Where(x => x.TodoListId == query.TodoListId)
            .OrderBy(x => x.SortOrder)
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
            .ToListAsync(cancellationToken);

        return items;
    }
}
