using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.Customers.CreateCustomer;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

/// <summary>
/// Handler for creating a new customer using the Customer aggregate factory method.
/// </summary>
/// <remarks>
/// Responsibility: Create a new customer entity with tenant and audit tracking.
/// 
/// Execution Flow:
/// 1. Use Customer.Create() factory method with current user context (tenant, userId, userName)
/// 2. Add entity to Customers DbSet
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return the created customer ID for result mapping
/// 
/// Multi-Tenancy: Tenant ID derived from ICurrentUser context (GetTenant() ?? "root")
/// Audit Trail: CreatedBy and CreatedOnUtc tracked via factory method
/// 
/// Permissions: Requires authenticated user with customer creation permission
/// 
/// Exceptions:
/// - NotFoundException: Not thrown; new entities cannot be missing
/// - BadRequestException: Thrown by validation if Name is empty or invalid
/// - DbException: Thrown if unique constraint violation occurs (duplicate customer name per tenant)
/// </remarks>
public class CreateCustomerHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var entity = Customer.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Customers.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
