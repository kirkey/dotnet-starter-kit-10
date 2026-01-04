using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.CreateWriteOff;

public record CreateWriteOffCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateWriteOffHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateWriteOffCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateWriteOffCommand command, CancellationToken ct)
    {
        var entity = WriteOff.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.WriteOffs.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
