using FSH.Modules.Todos.Contracts.v1.TodoTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todos.Features.v1.TodoTasks.GetTodoTasks;

public static class GetTodoTasksEndpoint
{
    public static RouteHandlerBuilder MapGetTodoTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{todoId:guid}/tasks", async (
            Guid todoId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodoTasksQuery(todoId);
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTodoTasksEndpoint))
        .WithSummary("Get tasks for a todo")
        .WithDescription("Retrieves all tasks for a specific todo, ordered by sort order")
        .Produces<List<TodoTaskDto>>(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
