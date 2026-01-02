using FSH.Modules.Todo.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.CompleteTodo;

public static class CompleteTodoEndpoint
{
    public static RouteHandlerBuilder MapCompleteTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/complete", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(CompleteTodoEndpoint))
        .WithSummary("Toggle todo completion")
        .WithDescription("Marks a todo as completed or reopens it if already completed")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
