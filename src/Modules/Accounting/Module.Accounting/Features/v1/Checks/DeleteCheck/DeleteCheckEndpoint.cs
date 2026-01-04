using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.DeleteCheck;

public static class DeleteCheckEndpoint
{
    public static RouteHandlerBuilder MapDeleteCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCheckCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCheckEndpoint))
        .WithSummary("Delete Check")
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Delete);
    }
}