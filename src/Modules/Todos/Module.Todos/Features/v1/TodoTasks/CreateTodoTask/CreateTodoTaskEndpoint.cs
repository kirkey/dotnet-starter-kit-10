using FSH.Module.Todos.Contracts.v1.TodoTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.TodoTasks.CreateTodoTask;

public static class CreateTodoTaskEndpoint
{
    public static RouteHandlerBuilder MapCreateTodoTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{todoId:guid}/tasks", async (
            Guid todoId,
            CreateTodoTaskCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (todoId != command.TodoId)
            {
                return Results.BadRequest("TodoId mismatch");
            }

            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/todo/{todoId}/tasks/{id}", id);
        })
        .WithName(nameof(CreateTodoTaskEndpoint))
        .WithSummary("Create a new task")
        .WithDescription("Creates a new task under a todo")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }
}
