using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.GeneralLedger;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.GeneralLedger.GetGeneralLedger;

public static class GetGeneralLedgerEndpoint
{
    public static RouteHandlerBuilder MapGetGeneralLedgerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetGeneralLedgerQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetGeneralLedgerEndpoint))
        .WithSummary("Get GeneralLedger by ID")
        .Produces<GeneralLedgerDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.GeneralLedger.View);
    }
}
