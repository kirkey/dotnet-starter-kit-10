using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todo.Features.v1.Todos.ExportTodos;

public static class ExportTodosEndpoint
{
    public static RouteHandlerBuilder MapExportTodosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/export", async (
            string format,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new ExportTodosQuery(format);
            var result = await mediator.Send(query, cancellationToken);
            
            return Results.File(result.Data, result.ContentType, result.FileName);
        })
        .WithName(nameof(ExportTodosEndpoint))
        .WithTags("Todos")
        .WithSummary("Export todos to CSV or JSON")
        .Produces(StatusCodes.Status200OK, contentType: "text/csv")
        .Produces(StatusCodes.Status200OK, contentType: "application/json")
        .RequirePermission(TodoPermissionConstants.Todos.Export);
    }
}
