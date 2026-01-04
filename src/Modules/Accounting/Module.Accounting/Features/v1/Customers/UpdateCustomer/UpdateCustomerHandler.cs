using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Customers.UpdateCustomer;

/// <summary>
/// Command to update an existing customer's metadata.
/// </summary>
/// <param name="Id">Customer ID to update (must exist)</param>
/// <param name="Name">Updated customer name</param>
/// <param name="Description">Updated customer description or null to clear</param>
public record UpdateCustomerCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

/// <summary>
/// Handler for updating customer metadata (Name, Description).
/// </summary>
/// <remarks>
/// Responsibility: Update customer mutable fields and persist changes.
/// 
/// Execution Flow:
/// 1. Find customer by Id using FindAsync(); throw NotFoundException if not found
/// 2. Call entity.Update(Name, Description) to apply changes
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return updated customer ID
/// 
/// Updateable Fields: Name, Description
/// Immutable Fields: Id, TenantId, CreatedBy, CreatedOnUtc, LastModifiedOnUtc, LastModifiedBy
/// 
/// Permissions: Requires authenticated user with customer update permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if customer with specified ID not found
/// - BadRequestException: Thrown by validation if Name is empty
/// - DbException: Thrown if unique constraint violation occurs
/// </remarks>
public class UpdateCustomerHandler(AccountingDbContext context) : ICommandHandler<UpdateCustomerCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCustomerCommand command, CancellationToken ct)
    {
        var entity = await context.Customers.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Customer not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
