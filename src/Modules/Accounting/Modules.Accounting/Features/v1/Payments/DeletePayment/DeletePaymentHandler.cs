using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Payments.DeletePayment;

public record DeletePaymentCommand(Guid Id) : ICommand;

public class DeletePaymentHandler(AccountingDbContext context) : ICommandHandler<DeletePaymentCommand>
{
    public async ValueTask<Unit> Handle(DeletePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");
        
        context.Payments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
