using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.GetTodoTasks;

public sealed class GetTodoTasksQueryHandler(TodoDbContext context)
    : IQueryHandler<GetTodoTasksQuery, List<TodoTaskDto>>
{
    public async ValueTask<List<TodoTaskDto>> Handle(GetTodoTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await context.TodoTasks
            .Where(t => t.TodoId == query.TodoId)
            .OrderBy(t => t.SortOrder)
            .ThenBy(t => t.CreatedOnUtc)
            .AsNoTracking()
            .Select(t => new TodoTaskDto(
                t.Id,
                t.TodoId,
                t.Name,
                t.Description,
                t.Status,
                t.IsCompleted,
                t.CompletedAt,
                t.SortOrder,
                t.CreatedOnUtc))
            .ToListAsync(cancellationToken);

        return tasks;
    }
}
