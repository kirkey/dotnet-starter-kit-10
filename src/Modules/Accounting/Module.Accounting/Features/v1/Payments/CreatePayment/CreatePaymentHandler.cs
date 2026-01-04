using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.Payments.CreatePayment;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.CreatePayment;

/// <summary>
/// Handler for creating a new Payment.
/// 
/// **Responsibility:**
/// Processes the CreatePaymentCommand by creating a new Payment aggregate.
/// Initializes payment with no allocations (amount applied is zero initially).
/// 
/// **Execution Flow:**
/// 1. Call Payment.Create() factory method with command data
/// 2. Initialize payment status (Draft or Active)
/// 3. Set amount applied to zero (allocations added separately)
/// 4. Record creation user and tenant
/// 5. Add to DbSet and persist
/// 6. Return new payment ID
/// 
/// **Initial State:**
/// - Status: Active (or Draft, depending on workflow)
/// - AmountApplied: 0 (updated as allocations are added)
/// - Allocations: Empty collection (managed separately)
/// - IsActive: true
/// 
/// **Dependencies:**
/// - AccountingDbContext: For database persistence
/// - ICurrentUser: For accessing current user context
/// </summary>
public class CreatePaymentHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePaymentCommand, Guid>
{
    /// <summary>
    /// Handles the CreatePaymentCommand to create a new payment.
    /// </summary>
    /// <param name="command">The command containing payment creation details</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the newly created payment</returns>
    public async ValueTask<Guid> Handle(CreatePaymentCommand command, CancellationToken ct)
    {
        var entity = Payment.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Payments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
