using FSH.Modules.Todo.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.DeleteTodo;

public static class DeleteTodoEndpoint
{
    public static RouteHandlerBuilder MapDeleteTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTodoCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteTodoEndpoint))
        .WithSummary("Delete a todo")
        .WithDescription("Deletes a todo and all its tasks")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
