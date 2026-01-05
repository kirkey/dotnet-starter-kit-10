using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.CreateCustomerCase;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.CreateCustomerCase;

public static class CreateCustomerCaseEndpoint
{
    public static RouteHandlerBuilder MapCreateCustomerCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCustomerCaseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCustomerCaseEndpoint))
        .WithSummary("Create CustomerCase")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CustomerCases.Create);
    }
}
