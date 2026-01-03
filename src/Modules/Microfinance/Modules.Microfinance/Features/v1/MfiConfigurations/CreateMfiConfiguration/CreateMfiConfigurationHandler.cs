using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.MfiConfigurations.CreateMfiConfiguration;

public record CreateMfiConfigurationCommand(string Name) : ICommand<Guid>;

public class CreateMfiConfigurationHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateMfiConfigurationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMfiConfigurationCommand command, CancellationToken ct)
    {
        var entity = MfiConfiguration.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.MfiConfigurations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
