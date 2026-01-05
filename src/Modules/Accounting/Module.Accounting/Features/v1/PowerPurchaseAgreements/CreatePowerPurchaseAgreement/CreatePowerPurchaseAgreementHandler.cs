using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

public class CreatePowerPurchaseAgreementHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePowerPurchaseAgreementCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePowerPurchaseAgreementCommand command, CancellationToken ct)
    {
        var entity = PowerPurchaseAgreement.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.PowerPurchaseAgreements.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
