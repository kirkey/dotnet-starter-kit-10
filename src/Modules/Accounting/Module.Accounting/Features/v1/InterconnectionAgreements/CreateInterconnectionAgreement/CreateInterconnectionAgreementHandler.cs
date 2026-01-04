using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

public record CreateInterconnectionAgreementCommand(string Name, string? Description) : ICommand<Guid>;

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
