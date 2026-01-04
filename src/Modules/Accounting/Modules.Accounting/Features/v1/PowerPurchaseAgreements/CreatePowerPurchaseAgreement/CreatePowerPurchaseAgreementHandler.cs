using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

public record CreatePowerPurchaseAgreementCommand(string Name, string? Description) : ICommand<Guid>;

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
