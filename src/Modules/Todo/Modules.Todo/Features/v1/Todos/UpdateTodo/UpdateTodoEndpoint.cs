using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Todo.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.UpdateTodo;

public static class UpdateTodoEndpoint
{
    public static RouteHandlerBuilder MapUpdateTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateTodoCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }

            var result = await mediator.Send(command, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateTodoEndpoint))
        .WithSummary("Update a todo")
        .WithDescription("Updates an existing todo item")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .RequirePermission(TodoPermissionConstants.Todos.Update);
    }
}
