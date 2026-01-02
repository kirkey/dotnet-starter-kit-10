using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Todo.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.CreateTodo;

public static class CreateTodoEndpoint
{
    public static RouteHandlerBuilder MapCreateTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateTodoCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/todo/{id}", id);
        })
        .WithName(nameof(CreateTodoEndpoint))
        .WithSummary("Create a new todo")
        .WithDescription("Creates a new todo item")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(TodoPermissionConstants.Todos.Create);
    }
}
