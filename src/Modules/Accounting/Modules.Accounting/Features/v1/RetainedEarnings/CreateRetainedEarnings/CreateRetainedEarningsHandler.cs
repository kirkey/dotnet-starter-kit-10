using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RetainedEarnings.CreateRetainedEarnings;

public record CreateRetainedEarningsCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateRetainedEarningsHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateRetainedEarningsCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRetainedEarningsCommand command, CancellationToken ct)
    {
        var entity = RetainedEarnings.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.RetainedEarnings.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
