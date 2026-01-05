// TODO: Implement Generate endpoint for TrialBalance
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.TrialBalance.GenerateTrialBalance;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;

public static class GenerateTrialBalanceEndpoint
{
    public static RouteHandlerBuilder MapGenerateTrialBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new GenerateTrialBalanceCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(GenerateTrialBalanceEndpoint))
        .WithSummary("Generate TrialBalance")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TrialBalance.Generate);
    }
}
