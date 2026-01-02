using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.UpdateTodoItem;

public static class UpdateTodoItemEndpoint
{
    public static RouteHandlerBuilder MapUpdateTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/items/{id:guid}",
            async Task<Results<NoContent, ValidationProblem>>
            (Guid id, UpdateTodoItemCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var updateCommand = command with { Id = id };
                await mediator.Send(updateCommand, ct);
                return TypedResults.NoContent();
            })
            .WithName("UpdateTodoItem")
            .WithSummary("Update a Todo Item")
            .WithDescription("Update an existing Todo Item")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem();
    }
}
