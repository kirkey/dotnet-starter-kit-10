using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payments.UpdatePayment;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.UpdatePayment;

/// <summary>
/// Handler for updating a Payment.
/// 
/// **Responsibility:**
/// Updates Payment metadata (name and description only).
/// 
/// **Execution Flow:**
/// 1. Find Payment by ID, throw NotFoundException if not found
/// 2. Call entity.Update() domain method with new values
/// 3. Persist changes to database
/// 4. Return updated payment ID
/// 
/// **Updateable Fields:**
/// - Name: Payment identifier
/// - Description: Optional description
/// 
/// **Immutable Fields:**
/// - Id: Cannot change
/// - AmountApplied: Managed via allocations
/// - Allocations: Managed separately
/// 
/// **Permissions:**
/// Requires: Accounting.Payment.Edit
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when payment not found
/// </summary>
public class UpdatePaymentHandler(AccountingDbContext context) : ICommandHandler<UpdatePaymentCommand, Guid>
{
    /// <summary>
    /// Handles the UpdatePaymentCommand to update payment metadata.
    /// </summary>
    /// <param name="command">The command containing the Payment updates</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the updated Payment</returns>
    /// <exception cref="NotFoundException">Thrown when Payment is not found</exception>
    public async ValueTask<Guid> Handle(UpdatePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
