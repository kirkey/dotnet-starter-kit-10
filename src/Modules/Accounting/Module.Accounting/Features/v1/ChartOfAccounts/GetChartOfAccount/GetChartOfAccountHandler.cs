using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccount;

/// <summary>
/// Get Chart of Account query DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a specific Chart of Account by its ID.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Chart of Account to retrieve
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// 
/// **Validation:**
/// The handler validates:
/// - Account exists in the current tenant
/// - Account ID is valid and not deleted
/// </summary>
public record GetChartOfAccountQuery(Guid Id) : IQuery<ChartOfAccountDto>;

/// <summary>
/// Handler for retrieving a single Chart of Account by ID.
/// 
/// **Responsibility:**
/// Queries the database for a Chart of Account by ID and returns it as a DTO.
/// 
/// **Execution Flow:**
/// 1. Query DbSet for ChartOfAccount by ID
/// 2. Project to ChartOfAccountDto with all properties
/// 3. Throw NotFoundException if not found
/// 4. Return the DTO
/// 
/// **Returned Fields:**
/// - Id, AccountCode, AccountName, AccountType, UsoaCategory
/// - ParentAccountId, ParentCode, Balance, IsControlAccount
/// - NormalBalance, AccountLevel, AllowDirectPosting
/// - IsUsoaCompliant, RegulatoryClassification, Description, Notes
/// - IsActive, CreatedOnUtc
/// 
/// **Permissions:**
/// Requires: Accounting.ChartOfAccount.View
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when ChartOfAccount is not found
/// </summary>
public class GetChartOfAccountHandler(AccountingDbContext context) : IQueryHandler<GetChartOfAccountQuery, ChartOfAccountDto>
{
    /// <summary>
    /// Handles the GetChartOfAccountQuery to retrieve a Chart of Account.
    /// </summary>
    /// <param name="query">The query containing the Account ID to retrieve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ChartOfAccountDto with all account details</returns>
    /// <exception cref="NotFoundException">Thrown when ChartOfAccount with the specified ID is not found</exception>
    public async ValueTask<ChartOfAccountDto> Handle(GetChartOfAccountQuery query, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts
            .Where(x => x.Id == query.Id)
            .Select(x => new ChartOfAccountDto(
                x.Id,
                x.AccountCode,
                x.AccountName,
                x.AccountType,
                x.UsoaCategory,
                x.ParentAccountId,
                x.ParentCode,
                x.Balance,
                x.IsControlAccount,
                x.NormalBalance,
                x.AccountLevel,
                x.AllowDirectPosting,
                x.IsUsoaCompliant,
                x.RegulatoryClassification,
                x.Description,
                x.Notes,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ChartOfAccount not found");
    }
}
