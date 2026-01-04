using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.DeleteInterconnectionAgreement;

public record DeleteInterconnectionAgreementCommand(Guid Id) : ICommand;

public class DeleteInterconnectionAgreementHandler(AccountingDbContext context) : ICommandHandler<DeleteInterconnectionAgreementCommand>
{
    public async ValueTask<Unit> Handle(DeleteInterconnectionAgreementCommand command, CancellationToken ct)
    {
        var entity = await context.InterconnectionAgreements.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InterconnectionAgreement not found");
        
        // Prevent deletion if there is generation history or outstanding credits
        if (entity.LifetimeGeneration > 0 || entity.YearToDateGeneration > 0 || entity.CurrentCreditBalance != 0m)
            throw new BadRequestException("Cannot delete interconnection agreement with generation history or outstanding credits");

        context.InterconnectionAgreements.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
