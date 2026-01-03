using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.QrPayments.CreateQrPayment;

public record CreateQrPaymentCommand(string Name) : ICommand<Guid>;

public class CreateQrPaymentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateQrPaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateQrPaymentCommand command, CancellationToken ct)
    {
        var entity = QrPayment.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.QrPayments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
