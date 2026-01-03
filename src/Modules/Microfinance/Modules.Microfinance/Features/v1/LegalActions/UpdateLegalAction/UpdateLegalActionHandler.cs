using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LegalActions.UpdateLegalAction;

public record UpdateLegalActionCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLegalActionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLegalActionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLegalActionCommand command, CancellationToken ct)
    {
        var entity = await context.LegalActions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LegalAction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
