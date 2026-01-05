using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.DeleteQrPayment;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.DeleteQrPayment;

public class DeleteQrPaymentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteQrPaymentCommand>
{
    public async ValueTask<Unit> Handle(DeleteQrPaymentCommand command, CancellationToken ct)
    {
        var entity = await context.QrPayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("QrPayment not found");
        
        context.QrPayments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
