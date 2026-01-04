using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.UpdateInterconnectionAgreement;

public static class UpdateInterconnectionAgreementEndpoint
{
    public static RouteHandlerBuilder MapUpdateInterconnectionAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInterconnectionAgreementCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateInterconnectionAgreementEndpoint))
        .WithSummary("Update InterconnectionAgreement")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterconnectionAgreements.Update);
    }
}
