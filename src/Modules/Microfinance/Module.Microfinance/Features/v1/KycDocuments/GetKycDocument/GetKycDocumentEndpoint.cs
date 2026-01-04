using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.KycDocuments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.GetKycDocument;

public static class GetKycDocumentEndpoint
{
    public static RouteHandlerBuilder MapGetKycDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetKycDocumentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetKycDocumentEndpoint))
        .WithSummary("Get KycDocument")
        .Produces<KycDocumentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.KycDocuments.View);
    }
}
