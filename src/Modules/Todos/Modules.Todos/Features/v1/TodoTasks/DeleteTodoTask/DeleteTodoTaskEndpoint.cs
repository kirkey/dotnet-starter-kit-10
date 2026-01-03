using FSH.Modules.Todos.Contracts.v1.TodoTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todos.Features.v1.TodoTasks.DeleteTodoTask;

public static class DeleteTodoTaskEndpoint
{
    public static RouteHandlerBuilder MapDeleteTodoTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/tasks/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTodoTaskCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteTodoTaskEndpoint))
        .WithSummary("Delete a task")
        .WithDescription("Deletes a task from a todo")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
