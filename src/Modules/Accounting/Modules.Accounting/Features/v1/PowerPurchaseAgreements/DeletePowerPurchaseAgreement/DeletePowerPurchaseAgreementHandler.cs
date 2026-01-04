using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PowerPurchaseAgreements.DeletePowerPurchaseAgreement;

public record DeletePowerPurchaseAgreementCommand(Guid Id) : ICommand;

public class DeletePowerPurchaseAgreementHandler(AccountingDbContext context) : ICommandHandler<DeletePowerPurchaseAgreementCommand>
{
    public async ValueTask<Unit> Handle(DeletePowerPurchaseAgreementCommand command, CancellationToken ct)
    {
        var entity = await context.PowerPurchaseAgreements.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PowerPurchaseAgreement not found");
        
        context.PowerPurchaseAgreements.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
