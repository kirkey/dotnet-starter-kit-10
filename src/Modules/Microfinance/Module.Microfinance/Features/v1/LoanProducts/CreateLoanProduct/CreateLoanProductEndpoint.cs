using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.CreateLoanProduct;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.CreateLoanProduct;

public static class CreateLoanProductEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanProductCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanProductEndpoint))
        .WithSummary("Create LoanProduct")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanProducts.Create);
    }
}
