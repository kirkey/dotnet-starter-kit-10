using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.UpdateMfiConfiguration;

public record UpdateMfiConfigurationCommand(Guid Id, string Name) : ICommand<Guid>;

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
