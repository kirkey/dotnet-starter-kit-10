using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.UpdatePowerPurchaseAgreement;

public record UpdatePowerPurchaseAgreementCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdatePowerPurchaseAgreementHandler(AccountingDbContext context) : ICommandHandler<UpdatePowerPurchaseAgreementCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePowerPurchaseAgreementCommand command, CancellationToken ct)
    {
        var entity = await context.PowerPurchaseAgreements.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PowerPurchaseAgreement not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
