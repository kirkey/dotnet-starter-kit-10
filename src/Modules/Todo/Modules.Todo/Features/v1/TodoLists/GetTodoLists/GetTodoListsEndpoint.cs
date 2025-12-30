using FSH.Modules.Todo.Contracts.v1.TodoLists;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoLists.GetTodoLists;

public static class GetTodoListsEndpoint
{
    public static RouteHandlerBuilder MapGetTodoListsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/lists",
            async Task<Ok<TodoListsResponse>>
            ([AsParameters] GetTodoListsQuery query, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(query, ct);
                return TypedResults.Ok(result);
            })
            .WithName("GetTodoLists")
            .WithTags("Todo Lists")
            .WithSummary("Get all Todo Lists")
            .WithDescription("Gets paginated list of Todo Lists for the current tenant")
            .RequireAuthorization()
            .Produces<TodoListsResponse>();
    }
}
