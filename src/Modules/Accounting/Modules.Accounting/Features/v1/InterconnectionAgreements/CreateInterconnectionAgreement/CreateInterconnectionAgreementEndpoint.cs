using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

public static class CreateInterconnectionAgreementEndpoint
{
    public static RouteHandlerBuilder MapCreateInterconnectionAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInterconnectionAgreementCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateInterconnectionAgreementEndpoint))
        .WithSummary("Create InterconnectionAgreement")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterconnectionAgreements.Create);
    }
}
