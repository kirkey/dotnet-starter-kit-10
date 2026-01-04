using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.UpdateInterconnectionAgreement;

public record UpdateInterconnectionAgreementCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateInterconnectionAgreementHandler(AccountingDbContext context) : ICommandHandler<UpdateInterconnectionAgreementCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInterconnectionAgreementCommand command, CancellationToken ct)
    {
        var entity = await context.InterconnectionAgreements.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InterconnectionAgreement not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
