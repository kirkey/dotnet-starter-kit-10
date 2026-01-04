using FSH.Module.Todos.Contracts.v1.TodoTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.TodoTasks.ToggleTaskCompletion;

public static class ToggleTaskCompletionEndpoint
{
    public static RouteHandlerBuilder MapToggleTaskCompletionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/tasks/{id:guid}/toggle", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ToggleTaskCompletionCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(ToggleTaskCompletionEndpoint))
        .WithSummary("Toggle task completion")
        .WithDescription("Marks a task as completed or reopens it if already completed")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
