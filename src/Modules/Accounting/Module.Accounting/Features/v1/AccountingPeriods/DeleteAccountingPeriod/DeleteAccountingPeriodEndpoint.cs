using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.DeleteAccountingPeriod;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.DeleteAccountingPeriod;

public static class DeleteAccountingPeriodEndpoint
{
    public static RouteHandlerBuilder MapDeleteAccountingPeriodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAccountingPeriodCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAccountingPeriodEndpoint))
        .WithSummary("Delete AccountingPeriod")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountingPeriods.Delete);
    }
}
