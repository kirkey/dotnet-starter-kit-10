using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.UpdateFeePayment;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.UpdateFeePayment;

public class UpdateFeePaymentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateFeePaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFeePaymentCommand command, CancellationToken ct)
    {
        var entity = await context.FeePayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeePayment not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
