using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.DeleteCreditBureauInquiry;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.DeleteCreditBureauInquiry;

public static class DeleteCreditBureauInquiryEndpoint
{
    public static RouteHandlerBuilder MapDeleteCreditBureauInquiryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCreditBureauInquiryCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCreditBureauInquiryEndpoint))
        .WithSummary("Delete CreditBureauInquiry")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauInquirys.Delete);
    }
}
