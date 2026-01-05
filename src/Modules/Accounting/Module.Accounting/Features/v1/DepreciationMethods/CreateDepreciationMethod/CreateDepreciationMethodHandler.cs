using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.DepreciationMethods.CreateDepreciationMethod;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.CreateDepreciationMethod;

public class CreateDepreciationMethodHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateDepreciationMethodCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDepreciationMethodCommand command, CancellationToken ct)
    {
        var entity = DepreciationMethod.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.DepreciationMethods.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
