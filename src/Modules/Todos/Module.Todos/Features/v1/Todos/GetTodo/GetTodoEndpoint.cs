using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.Todos.GetTodo;

public static class GetTodoEndpoint
{
    public static RouteHandlerBuilder MapGetTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodoQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTodoEndpoint))
        .WithSummary("Get todo by ID")
        .WithDescription("Retrieves a single todo with all its tasks")
        .Produces<TodoDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequirePermission(TodoPermissionConstants.Todos.View);
    }
}
