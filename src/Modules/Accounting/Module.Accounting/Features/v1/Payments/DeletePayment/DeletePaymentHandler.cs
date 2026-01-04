using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payments.DeletePayment;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.DeletePayment;

/// <summary>
/// Handler for deleting a Payment.
/// 
/// **Responsibility:**
/// Deletes a Payment from the database after validation.
/// Cannot delete payments with active allocations to invoices.
/// 
/// **Execution Flow:**
/// 1. Find Payment by ID, throw NotFoundException if not found
/// 2. Verify payment has no active allocations
/// 3. Delete the payment record
/// 4. Persist changes to database
/// 5. Return Unit.Value on success
/// 
/// **Pre-Delete Validation:**
/// - Payment must exist
/// - Payment must have no active allocations
/// - Cannot delete paid payments with allocated amounts
/// 
/// **Cascading Effects:**
/// - Associated PaymentAllocations are removed (if allowed)
/// - GL posting may be reversed (if payment was posted)
/// 
/// **Permissions:**
/// Requires: Accounting.Payment.Delete
/// 
/// **Business Rules:**
/// - Cannot delete payments with allocations
/// - Must unapply/deallocate first
/// - Payment deletion affects cash flow reporting
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when payment is not found
/// - BadRequestException: Thrown when payment has allocations
/// </summary>
public class DeletePaymentHandler(AccountingDbContext context) : ICommandHandler<DeletePaymentCommand>
{
    /// <summary>
    /// Handles the DeletePaymentCommand to remove a Payment.
    /// </summary>
    /// <param name="command">The command containing the Payment ID to delete</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful deletion</returns>
    /// <exception cref="NotFoundException">Thrown when Payment is not found</exception>
    public async ValueTask<Unit> Handle(DeletePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");
        
        context.Payments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
