using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.CreateSecurityDeposit;

public record CreateSecurityDepositCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateSecurityDepositHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateSecurityDepositCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateSecurityDepositCommand command, CancellationToken ct)
    {
        var entity = SecurityDeposit.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.SecurityDeposits.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
