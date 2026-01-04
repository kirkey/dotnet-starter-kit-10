// TODO: Implement Issue endpoint for Check
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Checks.IssueCheck;

public static class IssueCheckEndpoint
{
    public static RouteHandlerBuilder MapIssueCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new IssueCheckCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(IssueCheckEndpoint))
        .WithSummary("Issue Check")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Issue);
    }
}
