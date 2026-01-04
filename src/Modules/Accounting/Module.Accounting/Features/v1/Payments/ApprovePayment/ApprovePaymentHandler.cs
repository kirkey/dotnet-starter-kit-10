using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

/// <summary>
/// Approve Payment command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to approve a Payment.
/// Approval may be required before payment posting to GL.
/// 
/// **Parameters:**
/// - Id: The Payment ID to approve
/// 
/// **Business Constraints:**
/// - Payment must exist
/// - May require approval authority
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record ApprovePaymentCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for approving a Payment.
/// 
/// **Responsibility:**
/// Approves a Payment, recording approval metadata for audit trail.
/// 
/// **Execution Flow:**
/// 1. Find Payment by ID
/// 2. Call entity.Approve() domain method (if implemented)
/// 3. Record approval date and approving user
/// 4. Persist changes to database
/// 5. Return Unit.Value on success
/// 
/// **Approval Process:**
/// - Sets approval date/time
/// - Records approving user ID
/// - Updates payment status
/// - May enable GL posting
/// 
/// **Permissions:**
/// Requires: Accounting.Payment.Approve
/// 
/// **Business Rules:**
/// - Approval may be required before GL posting
/// - May enforce approval hierarchies
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when payment not found
/// 
/// **Note:**
/// Implementation requires domain approval logic implementation
/// </summary>
public class ApprovePaymentHandler(AccountingDbContext context) 
    : ICommandHandler<ApprovePaymentCommand>
{
    /// <summary>
    /// Handles the ApprovePaymentCommand to approve a Payment.
    /// </summary>
    /// <param name="command">The command containing the Payment ID to approve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful approval</returns>
    public async ValueTask<Unit> Handle(ApprovePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");

        // TODO: Implement domain approval logic (set approved by, status, etc.)

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
