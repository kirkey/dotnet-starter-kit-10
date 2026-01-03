using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.PaymentGateways.DeletePaymentGateway;

public record DeletePaymentGatewayCommand(Guid Id) : ICommand;

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
