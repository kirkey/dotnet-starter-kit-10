using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.GetKycDocuments;
using FSH.Module.Microfinance.Contracts.v1.KycDocuments;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.GetKycDocuments;

public static class GetKycDocumentsEndpoint
{
    public static RouteHandlerBuilder MapGetKycDocumentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetKycDocumentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetKycDocumentsEndpoint))
        .WithSummary("Get KycDocuments")
        .Produces<KycDocumentsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.KycDocuments.Search);
    }
}
