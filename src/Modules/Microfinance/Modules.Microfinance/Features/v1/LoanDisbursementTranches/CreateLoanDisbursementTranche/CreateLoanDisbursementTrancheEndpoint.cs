using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

public static class CreateLoanDisbursementTrancheEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanDisbursementTrancheEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanDisbursementTrancheCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanDisbursementTrancheEndpoint))
        .WithSummary("Create LoanDisbursementTranche")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanDisbursementTranches.Create);
    }
}
