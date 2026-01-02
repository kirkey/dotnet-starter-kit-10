using FSH.Modules.Todo.Contracts.v1.TodoItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoItems.CreateTodoItem;

public static class CreateTodoItemEndpoint
{
    public static RouteHandlerBuilder MapCreateTodoItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/items",
            async Task<Results<Created<Guid>, ValidationProblem>>
            (CreateTodoItemCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var id = await mediator.Send(command, ct);
                return TypedResults.Created($"/api/v1/todo/items/{id}", id);
            })
            .WithName("CreateTodoItem")
            .WithSummary("Create a new Todo Item")
            .WithDescription("Creates a new Todo Item within a Todo List")
            .RequireAuthorization()
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}
