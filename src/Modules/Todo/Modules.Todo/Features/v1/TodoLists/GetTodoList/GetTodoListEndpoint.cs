using FSH.Modules.Todo.Contracts.v1.TodoLists;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoLists.GetTodoList;

public static class GetTodoListEndpoint
{
    public static RouteHandlerBuilder MapGetTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/lists/{id:guid}",
            async Task<Results<Ok<TodoListResponse>, NotFound>>
            (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetTodoListQuery(id), ct);
                return result is not null
                    ? TypedResults.Ok(result)
                    : TypedResults.NotFound();
            })
            .WithName("GetTodoList")
            .WithTags("Todo Lists")
            .WithSummary("Get Todo List by ID")
            .WithDescription("Gets a Todo List with all its items")
            .RequireAuthorization()
            .Produces<TodoListResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
}
