using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.PaymentGateways.CreatePaymentGateway;

public record CreatePaymentGatewayCommand(string Name) : ICommand<Guid>;

public class CreatePaymentGatewayHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreatePaymentGatewayCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePaymentGatewayCommand command, CancellationToken ct)
    {
        var entity = PaymentGateway.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.PaymentGateways.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
