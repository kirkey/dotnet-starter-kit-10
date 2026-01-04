using FSH.Module.Todos.Contracts.v1.TodoTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.TodoTasks.UpdateTodoTask;

public static class UpdateTodoTaskEndpoint
{
    public static RouteHandlerBuilder MapUpdateTodoTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/tasks/{id:guid}", async (
            Guid id,
            UpdateTodoTaskCommand command,
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
        .WithName(nameof(UpdateTodoTaskEndpoint))
        .WithSummary("Update a task")
        .WithDescription("Updates an existing task")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }
}
