using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRepayments.CreateLoanRepayment;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.CreateLoanRepayment;

public static class CreateLoanRepaymentEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanRepaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanRepaymentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanRepaymentEndpoint))
        .WithSummary("Create LoanRepayment")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanRepayments.Create);
    }
}
