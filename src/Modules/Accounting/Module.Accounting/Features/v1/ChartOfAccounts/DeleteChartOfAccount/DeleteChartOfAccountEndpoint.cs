using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.DeleteChartOfAccount;

public static class DeleteChartOfAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteChartOfAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteChartOfAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteChartOfAccountEndpoint))
        .WithSummary("Delete ChartOfAccount")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Delete);
    }
}
