using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.DeleteInterconnectionAgreement;

public static class DeleteInterconnectionAgreementEndpoint
{
    public static RouteHandlerBuilder MapDeleteInterconnectionAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInterconnectionAgreementCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInterconnectionAgreementEndpoint))
        .WithSummary("Delete InterconnectionAgreement")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterconnectionAgreements.Delete);
    }
}
