using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LegalActions.DeleteLegalAction;

public record DeleteLegalActionCommand(Guid Id) : ICommand;

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
