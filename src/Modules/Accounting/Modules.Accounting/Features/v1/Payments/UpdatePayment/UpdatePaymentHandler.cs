using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Payments.UpdatePayment;

public record UpdatePaymentCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdatePaymentHandler(AccountingDbContext context) : ICommandHandler<UpdatePaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
