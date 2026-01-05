using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.GetApprovalRequest;
using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequest;

public static class GetApprovalRequestEndpoint
{
    public static RouteHandlerBuilder MapGetApprovalRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApprovalRequestQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetApprovalRequestEndpoint))
        .WithSummary("Get ApprovalRequest")
        .Produces<ApprovalRequestDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalRequests.View);
    }
}
