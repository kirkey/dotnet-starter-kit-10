using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

public record CreateCustomerCommand(string Name, string? Description) : ICommand<Guid>;

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
