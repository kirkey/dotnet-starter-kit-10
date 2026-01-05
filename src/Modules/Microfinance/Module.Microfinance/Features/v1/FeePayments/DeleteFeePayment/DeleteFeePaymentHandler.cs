using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.DeleteFeePayment;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.DeleteFeePayment;

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
