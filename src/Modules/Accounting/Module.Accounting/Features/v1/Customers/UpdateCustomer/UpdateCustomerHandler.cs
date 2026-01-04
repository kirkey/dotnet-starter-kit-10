using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Customers.UpdateCustomer;

public record UpdateCustomerCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
