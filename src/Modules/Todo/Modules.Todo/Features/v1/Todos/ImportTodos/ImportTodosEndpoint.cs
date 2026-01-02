using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.ImportTodos;

public static class ImportTodosEndpoint
{
    public static RouteHandlerBuilder MapImportTodosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/import", async (
            IFormFile file,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (file.Length == 0)
            {
                return Results.BadRequest(new { error = "File is empty" });
            }

            using var stream = file.OpenReadStream();
            var command = new ImportTodosCommand(stream, file.ContentType);
            var result = await mediator.Send(command, cancellationToken);

            if (result.FailureCount > 0)
            {
                return Results.Ok(new
                {
                    successCount = result.SuccessCount,
                    failureCount = result.FailureCount,
                    errors = result.Errors,
                    message = $"Imported {result.SuccessCount} todos with {result.FailureCount} failures"
                });
            }

            return Results.Ok(new
            {
                successCount = result.SuccessCount,
                message = $"Successfully imported {result.SuccessCount} todos"
            });
        })
        .WithName(nameof(ImportTodosEndpoint))
        .WithTags("Todos")
        .WithSummary("Import todos from CSV or JSON file")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(TodoPermissionConstants.Todos.Import)
        .DisableAntiforgery();
    }
}
