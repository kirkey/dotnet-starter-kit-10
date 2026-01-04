using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;

/// <summary>
/// Endpoint for creating a new Chart of Account.
/// 
/// **HTTP Mapping:**
/// POST /api/v1/accounting/accounts
/// 
/// **Purpose:**
/// Provides the HTTP endpoint for creating a new account in the chart of accounts
/// with the provided account details, classification, and sub-account mapping.
/// 
/// **Security:**
/// Requires ChartOfAccounts.Create permission.
/// 
/// **Request Body:**
/// Expects a CreateChartOfAccountCommand JSON object with:
/// - Code (required): string - Unique account code (e.g., "1000", "2000")
/// - Name (required): string - Account name
/// - AccountType (required): string - Type of account (Asset, Liability, Equity, etc.)
/// - SubType (optional): string - Subtype within the account type
/// - Balance (required): decimal - Initial account balance
/// - Description (optional): string - Account description
/// - IsActive (optional): bool - Whether account is active
/// 
/// **Response:**
/// - Status 201: Created - Returns the ID of the newly created account
/// - Location Header: Points to /api/v1/accounting/accounts/{id}
/// - Status 400: Bad Request - For validation errors
/// </summary>
public static class CreateChartOfAccountEndpoint
{
    /// <summary>
    /// Maps the CreateChartOfAccount endpoint to the route group.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure.</param>
    /// <returns>A route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapCreateChartOfAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateChartOfAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateChartOfAccountEndpoint))
        .WithSummary("Create ChartOfAccount")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Create);
    }
}
