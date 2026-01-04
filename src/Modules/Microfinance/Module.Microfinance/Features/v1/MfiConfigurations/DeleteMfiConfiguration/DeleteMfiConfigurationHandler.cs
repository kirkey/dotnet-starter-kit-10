using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.DeleteMfiConfiguration;

public record DeleteMfiConfigurationCommand(Guid Id) : ICommand;

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
