using FSH.Modules.Todo.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.GetTodos;

public static class GetTodosEndpoint
{
    public static RouteHandlerBuilder MapGetTodosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            string? status,
            int? priority,
            bool? isCompleted,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodosQuery(page, pageSize, searchTerm, status, priority, isCompleted);
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTodosEndpoint))
        .WithSummary("Get paginated list of todos")
        .WithDescription("Retrieves todos with optional filtering by search term, status, priority, and completion state")
        .Produces<TodosPagedResponse>(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
