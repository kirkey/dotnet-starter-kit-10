using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.CreateQrPayment;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.CreateQrPayment;

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
