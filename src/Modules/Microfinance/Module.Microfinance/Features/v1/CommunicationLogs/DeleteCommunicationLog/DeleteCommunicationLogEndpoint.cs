using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.DeleteCommunicationLog;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.DeleteCommunicationLog;

public static class DeleteCommunicationLogEndpoint
{
    public static RouteHandlerBuilder MapDeleteCommunicationLogEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCommunicationLogCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCommunicationLogEndpoint))
        .WithSummary("Delete CommunicationLog")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationLogs.Delete);
    }
}
