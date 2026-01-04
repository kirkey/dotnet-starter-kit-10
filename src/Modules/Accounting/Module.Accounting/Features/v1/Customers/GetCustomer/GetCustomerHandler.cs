using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Customers;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Customers.GetCustomer;

/// <summary>
/// Query to retrieve a single customer by ID.
/// </summary>
/// <param name="Id">Customer ID (Guid) to retrieve</param>
public record GetCustomerQuery(Guid Id) : IQuery<CustomerDto>;

/// <summary>
/// Handler for retrieving a single customer by ID with DTO projection.
/// </summary>
/// <remarks>
/// Responsibility: Execute the customer query and return a DTO with 5 returned fields.
/// 
/// Execution Flow:
/// 1. Query Customers DbSet by Id using Where(x => x.Id == query.Id)
/// 2. Project to CustomerDto with fields: Id, Name, Description, IsActive, CreatedOnUtc
/// 3. Execute FirstOrDefaultAsync() to retrieve single result
/// 4. Throw NotFoundException if entity not found
/// 
/// Returned Fields (CustomerDto): Id, Name, Description, IsActive, CreatedOnUtc
/// 
/// Permissions: Requires authenticated user (any authorized role)
/// 
/// Exceptions:
/// - NotFoundException: Thrown if customer with specified ID not found
/// </remarks>
public class GetCustomerHandler(AccountingDbContext context) : IQueryHandler<GetCustomerQuery, CustomerDto>
{
    public async ValueTask<CustomerDto> Handle(GetCustomerQuery query, CancellationToken ct)
    {
        var entity = await context.Customers
            .Where(x => x.Id == query.Id)
            .Select(x => new CustomerDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Customer not found");
    }
}
