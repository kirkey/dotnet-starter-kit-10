using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Customers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Customers.GetCustomer;

public static class GetCustomerEndpoint
{
    public static RouteHandlerBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCustomerEndpoint))
        .WithSummary("Get Customer by ID")
        .Produces<CustomerDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Customers.View);
    }
}
