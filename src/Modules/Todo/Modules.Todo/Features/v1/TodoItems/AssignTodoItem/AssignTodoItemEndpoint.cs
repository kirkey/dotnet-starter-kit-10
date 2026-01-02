using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.AssignTodoItem;

public static class AssignTodoItemEndpoint
{
    public static RouteHandlerBuilder MapAssignTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/items/{id:guid}/assign",
            async Task<Results<NoContent, NotFound>>
            (Guid id, AssignTodoItemCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var assignCommand = command with { Id = id };
                await mediator.Send(assignCommand, ct);
                return TypedResults.NoContent();
            })
            .WithName("AssignTodoItem")
            .WithSummary("Assign a Todo Item")
            .WithDescription("Assign a Todo Item to a user")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
