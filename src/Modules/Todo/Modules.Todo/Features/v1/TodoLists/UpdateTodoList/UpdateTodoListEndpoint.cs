using FSH.Modules.Todo.Contracts.v1.TodoLists;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.TodoLists.UpdateTodoList;

public static class UpdateTodoListEndpoint
{
    public static RouteHandlerBuilder MapUpdateTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/lists/{id:guid}",
            async Task<Results<Ok<bool>, NotFound, ValidationProblem>>
            (Guid id, UpdateTodoListCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var updateCommand = command with { Id = id };
                var result = await mediator.Send(updateCommand, ct);
                return result 
                    ? TypedResults.Ok(result)
                    : TypedResults.NotFound();
            })
            .WithName("UpdateTodoList")
            .WithTags("Todo Lists")
            .WithSummary("Update a Todo List")
            .WithDescription("Updates an existing Todo List")
            .RequireAuthorization()
            .Produces<bool>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }
}
