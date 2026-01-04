using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.CreatePayment;

public record CreatePaymentCommand(string Name, string? Description) : ICommand<Guid>;

public class CreatePaymentHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePaymentCommand, Guid>
{
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
