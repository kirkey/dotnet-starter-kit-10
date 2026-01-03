using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.InsurancePolicys.DeleteInsurancePolicy;

public static class DeleteInsurancePolicyEndpoint
{
    public static RouteHandlerBuilder MapDeleteInsurancePolicyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInsurancePolicyCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInsurancePolicyEndpoint))
        .WithSummary("Delete InsurancePolicy")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InsurancePolicys.Delete);
    }
}
