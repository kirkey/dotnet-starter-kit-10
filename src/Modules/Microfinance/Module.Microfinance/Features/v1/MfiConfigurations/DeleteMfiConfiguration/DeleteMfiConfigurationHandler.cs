using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.DeleteMfiConfiguration;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.DeleteMfiConfiguration;

public class DeleteMfiConfigurationHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMfiConfigurationCommand>
{
    public async ValueTask<Unit> Handle(DeleteMfiConfigurationCommand command, CancellationToken ct)
    {
        var entity = await context.MfiConfigurations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MfiConfiguration not found");
        
        context.MfiConfigurations.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
