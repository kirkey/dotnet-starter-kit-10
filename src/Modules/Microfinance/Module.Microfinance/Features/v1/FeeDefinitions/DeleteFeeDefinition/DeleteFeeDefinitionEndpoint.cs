using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.DeleteFeeDefinition;

public static class DeleteFeeDefinitionEndpoint
{
    public static RouteHandlerBuilder MapDeleteFeeDefinitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFeeDefinitionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFeeDefinitionEndpoint))
        .WithSummary("Delete FeeDefinition")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.FeeDefinitions.Delete);
    }
}
