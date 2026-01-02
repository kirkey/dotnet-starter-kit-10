using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.CompleteTodoItem;

public static class CompleteTodoItemEndpoint
{
    public static RouteHandlerBuilder MapCompleteTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/items/{id:guid}/complete",
            async Task<Results<NoContent, NotFound>>
            (Guid id, CompleteTodoItemCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var completeCommand = command with { Id = id };
                await mediator.Send(completeCommand, ct);
                return TypedResults.NoContent();
            })
            .WithName("CompleteTodoItem")
            .WithSummary("Complete a Todo Item")
            .WithDescription("Mark a Todo Item as completed")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
