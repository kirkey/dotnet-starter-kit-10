using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.PaymentGateways.UpdatePaymentGateway;

public record UpdatePaymentGatewayCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdatePaymentGatewayHandler(MicrofinanceDbContext context) : ICommandHandler<UpdatePaymentGatewayCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePaymentGatewayCommand command, CancellationToken ct)
    {
        var entity = await context.PaymentGateways.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("PaymentGateway not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
