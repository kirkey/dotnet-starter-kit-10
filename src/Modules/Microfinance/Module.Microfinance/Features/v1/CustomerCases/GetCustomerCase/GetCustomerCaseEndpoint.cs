using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.GetCustomerCase;
using FSH.Module.Microfinance.Contracts.v1.CustomerCases;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.GetCustomerCase;

public static class GetCustomerCaseEndpoint
{
    public static RouteHandlerBuilder MapGetCustomerCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerCaseQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCustomerCaseEndpoint))
        .WithSummary("Get CustomerCase")
        .Produces<CustomerCaseDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerCases.View);
    }
}
