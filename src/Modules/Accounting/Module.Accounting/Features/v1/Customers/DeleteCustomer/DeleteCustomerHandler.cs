using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Customers.DeleteCustomer;

public record DeleteCustomerCommand(Guid Id) : ICommand;

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
