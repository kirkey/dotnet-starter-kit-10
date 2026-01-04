using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

public static class CreateCreditBureauInquiryEndpoint
{
    public static RouteHandlerBuilder MapCreateCreditBureauInquiryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCreditBureauInquiryCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCreditBureauInquiryEndpoint))
        .WithSummary("Create CreditBureauInquiry")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauInquirys.Create);
    }
}
