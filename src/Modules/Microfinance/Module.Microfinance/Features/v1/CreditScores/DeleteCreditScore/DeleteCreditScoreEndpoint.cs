using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.DeleteCreditScore;

public static class DeleteCreditScoreEndpoint
{
    public static RouteHandlerBuilder MapDeleteCreditScoreEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCreditScoreCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCreditScoreEndpoint))
        .WithSummary("Delete CreditScore")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CreditScores.Delete);
    }
}
