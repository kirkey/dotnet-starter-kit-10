using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Banks.UpdateBank;

/// <summary>
/// Command to update an existing bank account's metadata and banking details.
/// </summary>
/// <param name="Id">Bank ID to update (must exist)</param>
/// <param name="BankName">Updated bank institution name (required)</param>
/// <param name="BankCode">Updated bank code or null</param>
/// <param name="Address">Updated address or null</param>
/// <param name="ContactName">Updated contact name or null</param>
/// <param name="ContactPhone">Updated contact phone or null</param>
/// <param name="RoutingNumber">Updated routing number or null</param>
/// <param name="SwiftCode">Updated SWIFT code or null</param>
/// <param name="CurrencyCode">Updated currency code or null</param>
/// <param name="OpeningBalance">Updated opening balance or null</param>
/// <param name="IsDefault">Updated default bank flag or null</param>
/// <param name="Description">Updated description or null</param>
public record UpdateBankCommand(
    Guid Id,
    string BankName,
    string? BankCode = null,
    string? Address = null,
    string? ContactName = null,
    string? ContactPhone = null,
    string? RoutingNumber = null,
    string? SwiftCode = null,
    string? CurrencyCode = null,
    decimal? OpeningBalance = null,
    bool? IsDefault = null,
    string? Description = null) : ICommand<Guid>;

/// <summary>
/// Handler for updating bank account metadata and banking details.
/// </summary>
/// <remarks>
/// Responsibility: Update bank mutable fields and persist changes.
/// 
/// Execution Flow:
/// 1. Find bank by Id using FindAsync(); throw NotFoundException if not found
/// 2. Call entity.Update() with all 11 parameters to apply changes
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return updated bank ID
/// 
/// Updateable Fields: BankName, BankCode, Address, ContactName, ContactPhone, RoutingNumber, SwiftCode, 
/// CurrencyCode, OpeningBalance, IsDefault, Description
/// Immutable Fields: Id, TenantId, CurrentBalance (changed only by transactions), CreatedBy, CreatedOnUtc
/// 
/// Notes: 
/// - CurrentBalance is managed by transaction posting (invoices, payments, journal entries)
/// - Opening balance can be adjusted but subsequent changes should use GL adjustment entries
/// - IsDefault can be used for automatic account selection in transaction processing
/// 
/// Permissions: Requires authenticated user with bank update permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if bank with specified ID not found
/// - BadRequestException: Thrown by validation if BankName is empty
/// </remarks>
public class UpdateBankHandler(AccountingDbContext context) : ICommandHandler<UpdateBankCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBankCommand command, CancellationToken ct)
    {
        var entity = await context.Banks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bank not found");
        
        entity.Update(
            command.BankName,
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
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
