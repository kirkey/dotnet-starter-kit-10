using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LegalActions.DeleteLegalAction;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.DeleteLegalAction;

public class DeleteLegalActionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLegalActionCommand>
{
    public async ValueTask<Unit> Handle(DeleteLegalActionCommand command, CancellationToken ct)
    {
        var entity = await context.LegalActions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LegalAction not found");
        
        context.LegalActions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
