using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.GetListInterconnectionAgreement;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreements;

public static class GetInterconnectionAgreementsEndpoint
{
    public static RouteHandlerBuilder MapGetInterconnectionAgreementsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetInterconnectionAgreementsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInterconnectionAgreementsEndpoint))
        .WithSummary("Get paginated list of InterconnectionAgreements")
        .Produces<InterconnectionAgreementsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterconnectionAgreements.Search);
    }
}
