using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.PaymentGateways.DeletePaymentGateway;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.DeletePaymentGateway;

public class DeletePaymentGatewayHandler(MicrofinanceDbContext context) : ICommandHandler<DeletePaymentGatewayCommand>
{
    public async ValueTask<Unit> Handle(DeletePaymentGatewayCommand command, CancellationToken ct)
    {
        var entity = await context.PaymentGateways.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("PaymentGateway not found");
        
        context.PaymentGateways.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
