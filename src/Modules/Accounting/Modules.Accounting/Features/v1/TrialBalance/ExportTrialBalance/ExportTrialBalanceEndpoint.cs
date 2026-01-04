// TODO: Implement Export endpoint for TrialBalance
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.ExportTrialBalance;

public static class ExportTrialBalanceEndpoint
{
    public static RouteHandlerBuilder MapExportTrialBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ExportTrialBalanceCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ExportTrialBalanceEndpoint))
        .WithSummary("Export TrialBalance")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TrialBalance.Export);
    }
}
