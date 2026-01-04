using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.DeleteInterconnectionAgreement;

public record DeleteInterconnectionAgreementCommand(Guid Id) : ICommand;

public class DeleteInterconnectionAgreementHandler(AccountingDbContext context) : ICommandHandler<DeleteInterconnectionAgreementCommand>
{
    public async ValueTask<Unit> Handle(DeleteInterconnectionAgreementCommand command, CancellationToken ct)
    {
        var entity = await context.InterconnectionAgreements.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InterconnectionAgreement not found");
        
        context.InterconnectionAgreements.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
