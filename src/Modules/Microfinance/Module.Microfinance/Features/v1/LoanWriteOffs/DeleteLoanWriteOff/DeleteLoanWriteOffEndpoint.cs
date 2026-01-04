using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.DeleteLoanWriteOff;

public static class DeleteLoanWriteOffEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanWriteOffCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanWriteOffEndpoint))
        .WithSummary("Delete LoanWriteOff")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanWriteOffs.Delete);
    }
}
