using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.CreateFeePayment;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.CreateFeePayment;

public class CreateFeePaymentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateFeePaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFeePaymentCommand command, CancellationToken ct)
    {
        var entity = FeePayment.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.FeePayments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
