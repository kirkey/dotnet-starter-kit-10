using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.CreateKycDocument;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.CreateKycDocument;

public static class CreateKycDocumentEndpoint
{
    public static RouteHandlerBuilder MapCreateKycDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateKycDocumentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateKycDocumentEndpoint))
        .WithSummary("Create KycDocument")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.KycDocuments.Create);
    }
}
