using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.GetTodo;

public sealed class GetTodoQueryHandler(TodoDbContext context)
    : IQueryHandler<GetTodoQuery, TodoDto>
{
    public async ValueTask<TodoDto> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todo = await context.Todos
            .Include(t => t.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == query.Id, cancellationToken);

        if (todo == null)
        {
            throw new InvalidOperationException($"Todo with ID {query.Id} not found");
        }

        return new TodoDto(
            todo.Id,
            todo.Name,
            todo.Description,
            todo.Notes,
            todo.Status,
            todo.IsActive,
            (int)todo.Priority,
            todo.DueDate,
            todo.IsCompleted,
            todo.CompletedAt,
            todo.Tasks.Count,
            todo.Tasks.Count(t => t.IsCompleted),
            todo.CreatedOnUtc,
            todo.CreatedByUserName);
    }
}
