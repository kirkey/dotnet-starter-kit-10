using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Payments.CreatePayment;

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
