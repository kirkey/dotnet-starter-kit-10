using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.CreateCheck;

/// <summary>
/// Command to create a new check payment with bank account and payee details.
/// </summary>
/// <param name="CheckNumber">Check number (required, unique per bank account)</param>
/// <param name="CheckDate">Check date/issuance date (required)</param>
/// <param name="CheckType">Check type classifier (e.g., "Personal", "Corporate", "Payroll")</param>
/// <param name="BankAccountId">Bank account ID from which check is drawn (required)</param>
/// <param name="AccountNumber">Bank account number (required for reconciliation)</param>
/// <param name="Amount">Check amount in bank account currency (required, positive)</param>
/// <param name="PayeeName">Payee name or company name (required)</param>
/// <param name="PayeeId">Optional reference to Customer/Vendor/Employee entity</param>
/// <param name="ReferenceNumber">Optional invoice/bill reference number for matching</param>
/// <param name="Notes">Optional memo or payment notes for recipient</param>
public record CreateCheckCommand(
    string CheckNumber,
    DateTime CheckDate,
    string CheckType,
    Guid BankAccountId,
    string AccountNumber,
    decimal Amount,
    string PayeeName,
    Guid? PayeeId = null,
    string? ReferenceNumber = null,
    string? Notes = null) : ICommand<Guid>;

/// <summary>
/// Handler for creating a new check using the Check aggregate factory method.
/// </summary>
/// <remarks>
/// Responsibility: Create a new check entity with bank and payee tracking, audit tracking.
/// 
/// Execution Flow:
/// 1. Use Check.Create() factory method with 10 parameters and current user context (tenant, userId, userName)
/// 2. Add entity to Checks DbSet
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return the created check ID for result mapping
/// 
/// Creation Parameters:
/// - CheckNumber: Unique identifier per bank account, formatted as check number
/// - CheckDate: Date check is written/issued
/// - CheckType: Classifier for internal routing and reporting
/// - BankAccountId: Required link to bank account entity (currency validation)
/// - AccountNumber: Bank account number for printed check detail
/// - Amount: Payment amount (must be positive, matches bank account currency)
/// - PayeeName: Recipient name (required for check printing)
/// - PayeeId: Optional reference to Customer/Vendor/Employee for reconciliation
/// - ReferenceNumber: Optional invoice number for AP matching
/// - Notes: Memo line or payment description
/// 
/// Initial State: Check created with Status='Draft' or 'Pending' awaiting print/issue
/// 
/// Multi-Tenancy: Tenant ID derived from ICurrentUser context (GetTenant() ?? "root")
/// Audit Trail: CreatedBy and CreatedOnUtc tracked via factory method
/// 
/// Permissions: Requires authenticated user with check creation permission
/// 
/// Exceptions:
/// - NotFoundException: Not thrown; new entities cannot be missing
/// - BadRequestException: Thrown by validation if CheckNumber/Amount invalid
/// - DbException: Thrown if foreign key constraint fails (BankAccountId not found)
/// </remarks>
public class CreateCheckHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateCheckCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCheckCommand command, CancellationToken ct)
    {
        var entity = Check.Create(
            command.CheckNumber,
            command.CheckDate,
            command.CheckType,
            command.BankAccountId,
            command.AccountNumber,
            command.Amount,
            command.PayeeName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.PayeeId,
            command.ReferenceNumber,
            command.Notes);

        context.Checks.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}