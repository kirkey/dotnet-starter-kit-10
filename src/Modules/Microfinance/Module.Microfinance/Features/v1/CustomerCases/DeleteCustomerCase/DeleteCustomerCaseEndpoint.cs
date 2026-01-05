using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.DeleteCustomerCase;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.DeleteCustomerCase;

public static class DeleteCustomerCaseEndpoint
{
    public static RouteHandlerBuilder MapDeleteCustomerCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCustomerCaseCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCustomerCaseEndpoint))
        .WithSummary("Delete CustomerCase")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CustomerCases.Delete);
    }
}
