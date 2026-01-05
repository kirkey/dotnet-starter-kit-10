using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


using FSH.Module.Microfinance.Contracts.v1.LoanApplications;

namespace FSH.Module.Microfinance.Features.v1.LoanApplications.CreateLoanApplication;

public static class CreateLoanApplicationEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanApplicationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanApplicationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanApplicationEndpoint))
        .WithSummary("Create LoanApplication")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanApplications.Create);
    }
}
