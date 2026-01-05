using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.GetInterconnectionAgreement;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreement;

public static class GetInterconnectionAgreementEndpoint
{
    public static RouteHandlerBuilder MapGetInterconnectionAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInterconnectionAgreementQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInterconnectionAgreementEndpoint))
        .WithSummary("Get InterconnectionAgreement by ID")
        .Produces<InterconnectionAgreementDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterconnectionAgreements.View);
    }
}
