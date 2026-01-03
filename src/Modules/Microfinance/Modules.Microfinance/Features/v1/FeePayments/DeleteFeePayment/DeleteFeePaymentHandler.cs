using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeePayments.DeleteFeePayment;

public record DeleteFeePaymentCommand(Guid Id) : ICommand;

public class DeleteFeePaymentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteFeePaymentCommand>
{
    public async ValueTask<Unit> Handle(DeleteFeePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.FeePayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeePayment not found");
        
        context.FeePayments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
