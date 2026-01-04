using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Banks.CreateBank;

/// <summary>
/// Command to create a new bank account entity with international banking details.
/// </summary>
/// <param name="BankName">Bank institution name (required, e.g., "Chase Bank", "HSBC")</param>
/// <param name="BankCode">Optional bank code (e.g., ABA code for US banks)</param>
/// <param name="Address">Optional bank branch address</param>
/// <param name="ContactName">Optional bank contact person name</param>
/// <param name="ContactPhone">Optional bank contact phone number</param>
/// <param name="RoutingNumber">Optional routing/ABA number for wire transfers</param>
/// <param name="SwiftCode">Optional SWIFT code for international transfers</param>
/// <param name="CurrencyCode">Optional currency code (e.g., "USD", "EUR"), defaults to tenant currency</param>
/// <param name="OpeningBalance">Initial account balance (default=0)</param>
/// <param name="IsDefault">Mark as default bank for automatic account reconciliation (default=false)</param>
/// <param name="Description">Optional bank account description or notes</param>
public record CreateBankCommand(
    string BankName,
    string? BankCode = null,
    string? Address = null,
    string? ContactName = null,
    string? ContactPhone = null,
    string? RoutingNumber = null,
    string? SwiftCode = null,
    string? CurrencyCode = null,
    decimal OpeningBalance = 0,
    bool IsDefault = false,
    string? Description = null) : ICommand<Guid>;

/// <summary>
/// Handler for creating a new bank account using the Bank aggregate factory method.
/// </summary>
/// <remarks>
/// Responsibility: Create a new bank account entity with international banking support and audit tracking.
/// 
/// Execution Flow:
/// 1. Use Bank.Create() factory method with 11 parameters and current user context (tenant, userId, userName)
/// 2. Add entity to Banks DbSet
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return the created bank ID for result mapping
/// 
/// Creation Parameters:
/// - BankName (required): Institution name for account identification
/// - BankCode: Optional internal bank code for routing/reconciliation
/// - Address, ContactName, ContactPhone: Optional bank contact details
/// - RoutingNumber: Optional routing number for domestic wire transfers
/// - SwiftCode: Optional SWIFT code for international transfers
/// - CurrencyCode: Optional currency (supports multi-currency accounting)
/// - OpeningBalance: Initial balance for opening balance adjustment entries
/// - IsDefault: Boolean flag for default bank account selection
/// - Description: Notes for account type, purpose, or business context
/// 
/// Multi-Tenancy: Tenant ID derived from ICurrentUser context (GetTenant() ?? "root")
/// Audit Trail: CreatedBy and CreatedOnUtc tracked via factory method
/// 
/// Permissions: Requires authenticated user with bank creation permission
/// 
/// Exceptions:
/// - NotFoundException: Not thrown; new entities cannot be missing
/// - BadRequestException: Thrown by validation if BankName is empty
/// - DbException: Thrown if unique constraint violation occurs (duplicate bank name per tenant)
/// </remarks>
public class CreateBankHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBankCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBankCommand command, CancellationToken ct)
    {
        var entity = Bank.Create(
            command.BankName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.BankCode,
            command.Address,
            command.ContactName,
            command.ContactPhone,
            command.RoutingNumber,
            command.SwiftCode,
            command.CurrencyCode,
            command.OpeningBalance,
            command.IsDefault,
            command.Description);
        
        context.Banks.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
