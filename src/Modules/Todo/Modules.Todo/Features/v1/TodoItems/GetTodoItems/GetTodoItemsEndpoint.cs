using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItems;

public static class GetTodoItemsEndpoint
{
    public static RouteHandlerBuilder MapGetTodoItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/lists/{todoListId:guid}/items",
            async (Guid todoListId, IMediator mediator, CancellationToken ct) =>
            {
                var items = await mediator.Send(new GetTodoItemsQuery(todoListId), ct);
                return TypedResults.Ok(items);
            })
            .WithName("GetTodoItems")
            .WithSummary("Get Todo Items")
            .WithDescription("Retrieve all Todo Items for a specific Todo List")
            .RequireAuthorization()
            .Produces<List<TodoItemResponse>>(StatusCodes.Status200OK);
    }
}
