using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.DepreciationMethods.UpdateDepreciationMethod;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.UpdateDepreciationMethod;

public class UpdateDepreciationMethodHandler(AccountingDbContext context) : ICommandHandler<UpdateDepreciationMethodCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDepreciationMethodCommand command, CancellationToken ct)
    {
        var entity = await context.DepreciationMethods.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DepreciationMethod not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
