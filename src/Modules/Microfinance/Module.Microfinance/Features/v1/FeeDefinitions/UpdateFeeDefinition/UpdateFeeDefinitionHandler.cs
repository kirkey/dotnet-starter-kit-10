using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.UpdateFeeDefinition;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.UpdateFeeDefinition;

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
