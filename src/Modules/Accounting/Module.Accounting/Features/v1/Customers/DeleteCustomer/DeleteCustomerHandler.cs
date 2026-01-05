using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Customers.DeleteCustomer;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Customers.DeleteCustomer;

/// <summary>
/// Handler for deleting a customer with business rule validation.
/// </summary>
/// <remarks>
/// Responsibility: Delete customer if no invoices exist; enforce referential integrity.
/// 
/// Execution Flow:
/// 1. Find customer by Id using FindAsync(); throw NotFoundException if not found
/// 2. Check for existing invoices using context.Invoices.AnyAsync(x => x.CustomerId == Id)
/// 3. Throw BadRequestException if customer has invoices (must be removed/reassigned first)
/// 4. Remove customer from Customers DbSet
/// 5. Persist deletion via SaveChangesAsync
/// 
/// Business Rules:
/// - Cannot delete customer with existing invoices
/// - Customer must be explicitly reassigned or invoices removed before deletion
/// - Cascading deletes handled by database constraints
/// 
/// Permissions: Requires authenticated user with customer delete permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if customer with specified ID not found
/// - BadRequestException: Thrown if customer has existing invoices (referential integrity constraint)
/// </remarks>
public class DeleteCustomerHandler(AccountingDbContext context) : ICommandHandler<DeleteCustomerCommand>
{
    public async ValueTask<Unit> Handle(DeleteCustomerCommand command, CancellationToken ct)
    {
        var entity = await context.Customers.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Customer not found");

        // Business rule: Cannot delete customers with invoices
        var hasInvoices = await context.Invoices.AnyAsync(x => x.CustomerId == command.Id, ct).ConfigureAwait(false);
        if (hasInvoices)
            throw new BadRequestException("Cannot delete customer with existing invoices. Please remove or reassign invoices first.");
        
        context.Customers.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
