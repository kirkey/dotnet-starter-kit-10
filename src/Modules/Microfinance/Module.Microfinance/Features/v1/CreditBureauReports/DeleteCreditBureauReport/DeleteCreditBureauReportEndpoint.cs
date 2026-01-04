using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.DeleteCreditBureauReport;

public static class DeleteCreditBureauReportEndpoint
{
    public static RouteHandlerBuilder MapDeleteCreditBureauReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCreditBureauReportCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCreditBureauReportEndpoint))
        .WithSummary("Delete CreditBureauReport")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauReports.Delete);
    }
}
