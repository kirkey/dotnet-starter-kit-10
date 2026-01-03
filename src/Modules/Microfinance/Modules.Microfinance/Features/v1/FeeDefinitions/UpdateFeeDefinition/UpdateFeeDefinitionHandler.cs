using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeDefinitions.UpdateFeeDefinition;

public record UpdateFeeDefinitionCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateFeeDefinitionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateFeeDefinitionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFeeDefinitionCommand command, CancellationToken ct)
    {
        var entity = await context.FeeDefinitions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeDefinition not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
