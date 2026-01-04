using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

public record ApprovePaymentCommand(Guid Id) : ICommand;

public class ApprovePaymentHandler(AccountingDbContext context) 
    : ICommandHandler<ApprovePaymentCommand>
{
    public async ValueTask<Unit> Handle(ApprovePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.Payments.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payment not found");

        // TODO: Implement domain approval logic (set approved by, status, etc.)

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
