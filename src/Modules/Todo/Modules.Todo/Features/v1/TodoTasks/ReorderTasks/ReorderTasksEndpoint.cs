using FSH.Modules.Todo.Contracts.v1.TodoTasks;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.ReorderTasks;

public static class ReorderTasksEndpoint
{
    public static RouteHandlerBuilder MapReorderTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPatch("/{todoId:guid}/tasks/reorder", async (
            Guid todoId,
            List<TaskOrderItem> tasks,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ReorderTasksCommand(todoId, tasks);
            await mediator.Send(command, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName(nameof(ReorderTasksEndpoint))
        .WithSummary("Reorder todo tasks")
        .WithDescription("Updates the sort order of multiple tasks in a todo")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
