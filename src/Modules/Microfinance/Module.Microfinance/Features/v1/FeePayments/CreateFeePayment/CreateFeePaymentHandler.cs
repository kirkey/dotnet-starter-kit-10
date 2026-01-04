using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.CreateFeePayment;

public record CreateFeePaymentCommand(string Name) : ICommand<Guid>;

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
