using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

public class CreateInterconnectionAgreementHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInterconnectionAgreementCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInterconnectionAgreementCommand command, CancellationToken ct)
    {
        var entity = InterconnectionAgreement.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.InterconnectionAgreements.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
