using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItem;

public static class GetTodoItemEndpoint
{
    public static RouteHandlerBuilder MapGetTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/items/{id:guid}",
            async Task<Results<Ok<TodoItemResponse>, NotFound>>
            (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var item = await mediator.Send(new GetTodoItemQuery(id), ct);
                return item is not null
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound();
            })
            .WithName("GetTodoItem")
            .WithSummary("Get a Todo Item")
            .WithDescription("Retrieve a specific Todo Item by ID")
            .RequireAuthorization()
            .Produces<TodoItemResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
