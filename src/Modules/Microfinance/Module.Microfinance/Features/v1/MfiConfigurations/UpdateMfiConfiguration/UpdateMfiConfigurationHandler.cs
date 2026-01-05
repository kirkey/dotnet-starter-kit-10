using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.UpdateMfiConfiguration;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.UpdateMfiConfiguration;

public class UpdateMfiConfigurationHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMfiConfigurationCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMfiConfigurationCommand command, CancellationToken ct)
    {
        var entity = await context.MfiConfigurations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MfiConfiguration not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
