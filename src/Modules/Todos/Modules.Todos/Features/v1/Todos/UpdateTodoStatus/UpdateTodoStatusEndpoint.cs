using FSH.Modules.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todos.Features.v1.Todos.UpdateTodoStatus;

public static class UpdateTodoStatusEndpoint
{
    public static RouteHandlerBuilder MapUpdateTodoStatusEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPatch("/{id:guid}/status", async (
            Guid id,
            int status,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTodoStatusCommand(id, status);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(UpdateTodoStatusEndpoint))
        .WithSummary("Update todo status")
        .WithDescription("Updates the status of a todo (0=NotStarted, 1=InProgress, 2=Completed, 3=OnHold)")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
