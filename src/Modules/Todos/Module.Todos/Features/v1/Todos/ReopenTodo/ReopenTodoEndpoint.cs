using FSH.Module.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.Todos.ReopenTodo;

public static class ReopenTodoEndpoint
{
    public static RouteHandlerBuilder MapReopenTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/reopen", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ReopenTodoCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(ReopenTodoEndpoint))
        .WithSummary("Reopen a completed todo")
        .WithDescription("Reopens a completed todo and sets its status back to InProgress")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
