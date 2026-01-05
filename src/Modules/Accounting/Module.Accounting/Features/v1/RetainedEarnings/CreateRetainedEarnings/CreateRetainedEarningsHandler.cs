using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using RetainedEarningsEntity = FSH.Module.Accounting.Domain.RetainedEarnings;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CreateRetainedEarnings;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CreateRetainedEarnings; 

public class CreateRetainedEarningsHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateRetainedEarningsCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRetainedEarningsCommand command, CancellationToken ct)
    {
        var entity = RetainedEarningsEntity.Create(
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
