using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.DeleteTodoItem;

public static class DeleteTodoItemEndpoint
{
    public static RouteHandlerBuilder MapDeleteTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/items/{id:guid}",
            async Task<Results<NoContent, NotFound>>
            (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteTodoItemCommand(id), ct);
                return TypedResults.NoContent();
            })
            .WithName("DeleteTodoItem")
            .WithSummary("Delete a Todo Item")
            .WithDescription("Delete an existing Todo Item")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
