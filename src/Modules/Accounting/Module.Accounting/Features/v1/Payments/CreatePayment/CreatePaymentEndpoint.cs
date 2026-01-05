using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Payments.CreatePayment;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.CreatePayment;

public static class CreatePaymentEndpoint
{
    public static RouteHandlerBuilder MapCreatePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreatePaymentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreatePaymentEndpoint))
        .WithSummary("Create Payment")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.Create);
    }
}
