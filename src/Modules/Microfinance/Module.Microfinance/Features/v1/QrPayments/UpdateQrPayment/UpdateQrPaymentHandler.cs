using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.UpdateQrPayment;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.UpdateQrPayment;

public class UpdateQrPaymentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateQrPaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateQrPaymentCommand command, CancellationToken ct)
    {
        var entity = await context.QrPayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("QrPayment not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
