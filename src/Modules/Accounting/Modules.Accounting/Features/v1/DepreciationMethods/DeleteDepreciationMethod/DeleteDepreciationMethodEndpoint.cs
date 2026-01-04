using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.DepreciationMethods.DeleteDepreciationMethod;

public static class DeleteDepreciationMethodEndpoint
{
    public static RouteHandlerBuilder MapDeleteDepreciationMethodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteDepreciationMethodCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteDepreciationMethodEndpoint))
        .WithSummary("Delete DepreciationMethod")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DepreciationMethods.Delete);
    }
}
