using FSH.Modules.Todo.Contracts.v1.TodoLists;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;

public static class CreateTodoListEndpoint
{
    public static RouteHandlerBuilder MapCreateTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/lists",
            async Task<Results<Created<Guid>, ValidationProblem>>
            (CreateTodoListCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var id = await mediator.Send(command, ct);
                return TypedResults.Created($"/api/v1/todo/lists/{id}", id);
            })
            .WithName("CreateTodoList")
            .WithSummary("Create a new Todo List")
            .WithDescription("Creates a new Todo List for the current tenant")
            .RequireAuthorization()
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}
