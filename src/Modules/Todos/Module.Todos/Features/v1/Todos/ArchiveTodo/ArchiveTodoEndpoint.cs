using FSH.Module.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.Todos.ArchiveTodo;

public static class ArchiveTodoEndpoint
{
    public static RouteHandlerBuilder MapArchiveTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/archive", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ArchiveTodoCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(ArchiveTodoEndpoint))
        .WithSummary("Archive a todo")
        .WithDescription("Archives a todo by setting IsActive to false")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
