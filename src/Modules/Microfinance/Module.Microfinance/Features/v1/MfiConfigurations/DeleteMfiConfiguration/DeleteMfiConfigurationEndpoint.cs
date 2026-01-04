using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.DeleteMfiConfiguration;

public static class DeleteMfiConfigurationEndpoint
{
    public static RouteHandlerBuilder MapDeleteMfiConfigurationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMfiConfigurationCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMfiConfigurationEndpoint))
        .WithSummary("Delete MfiConfiguration")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.MfiConfigurations.Delete);
    }
}
