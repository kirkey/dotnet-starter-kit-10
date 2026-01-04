using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.DepreciationMethods.UpdateDepreciationMethod;

public record UpdateDepreciationMethodCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
