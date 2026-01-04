using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Customers;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Customers.GetCustomer;

public record GetCustomerQuery(Guid Id) : IQuery<CustomerDto>;

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
