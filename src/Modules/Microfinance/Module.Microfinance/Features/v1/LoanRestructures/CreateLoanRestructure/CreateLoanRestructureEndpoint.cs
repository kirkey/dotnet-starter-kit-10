using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRestructures.CreateLoanRestructure;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.CreateLoanRestructure;

public static class CreateLoanRestructureEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanRestructureEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanRestructureCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanRestructureEndpoint))
        .WithSummary("Create LoanRestructure")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanRestructures.Create);
    }
}
