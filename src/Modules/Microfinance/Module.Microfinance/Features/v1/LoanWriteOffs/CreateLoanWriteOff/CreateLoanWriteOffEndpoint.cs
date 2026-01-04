using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.CreateLoanWriteOff;

public static class CreateLoanWriteOffEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanWriteOffCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanWriteOffEndpoint))
        .WithSummary("Create LoanWriteOff")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanWriteOffs.Create);
    }
}
