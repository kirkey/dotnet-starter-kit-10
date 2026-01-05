using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.Members.DeleteMember;

namespace FSH.Module.Accounting.Features.v1.Members.DeleteMember;

public static class DeleteMemberEndpoint
{
    public static RouteHandlerBuilder MapDeleteMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMemberCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMemberEndpoint))
        .WithSummary("Delete Member")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Members.Delete);
    }
}
