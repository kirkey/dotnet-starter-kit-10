using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Endpoint for creating a new todo item.
/// 
/// **HTTP Mapping:**
/// POST /api/v1/todo
/// 
/// **Purpose:**
/// Provides the HTTP endpoint for creating a new todo item with the provided details.
/// 
/// **Security:**
/// Requires Todos.Create permission.
/// 
/// **Request Body:**
/// Expects a CreateTodoCommand JSON object with:
/// - Name (required): string
/// - Description (optional): string
/// - Notes (optional): string
/// - Priority (required): int (1-4)
/// - DueDate (optional): DateTimeOffset
/// 
/// **Response:**
/// - Status 201: Created - Returns the ID of the newly created todo
/// - Location Header: Points to /api/v1/todo/{id}
/// - Status 400: Bad Request - For validation errors
/// </summary>
public static class CreateTodoEndpoint
{
    /// <summary>
    /// Maps the CreateTodo endpoint to the route group.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure.</param>
    /// <returns>A route handler builder for further configuration.</returns>
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
        .WithDescription("Creates a new todo item with the provided details")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(TodoPermissionConstants.Todos.Create);
    }
}
