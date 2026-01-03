using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Todos.Contracts.v1.Todos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todos.Features.v1.Todos.CompleteTodo;

/// <summary>
/// Endpoint for toggling the completion status of a todo.
/// 
/// **HTTP Mapping:**
/// POST /api/v1/todo/{id}/complete
/// 
/// **Purpose:**
/// Provides the HTTP endpoint for toggling todo completion status.
/// If the todo is completed, it will be reopened. If pending, it will be completed.
/// 
/// **Security:**
/// Requires Todos.Update permission.
/// 
/// **Response:**
/// - Status 204: No Content - Operation successful
/// - Status 404: Not Found - Todo not found
/// </summary>
public static class CompleteTodoEndpoint
{
    /// <summary>
    /// Maps the CompleteTodo endpoint to the route group.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure.</param>
    /// <returns>A route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapCompleteTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/complete", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoCommand(id);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(CompleteTodoEndpoint))
        .WithSummary("Toggle todo completion")
        .WithDescription("Marks a todo as completed or reopens it if already completed")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequirePermission(TodoPermissionConstants.Todos.Update);
    }
}
