using FSH.Modules.Todo.Contracts.v1.TodoLists;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoLists.DeleteTodoList;

public static class DeleteTodoListEndpoint
{
    public static RouteHandlerBuilder MapDeleteTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}",
            async Task<Results<NoContent, NotFound>>
            (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteTodoListCommand(id), ct);
                return TypedResults.NoContent();
            })
            .WithName("DeleteTodoList")
            .WithTags("Todos")
            .WithSummary("Delete a Todo List")
            .WithDescription("Delete an existing Todo List and all its items")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
