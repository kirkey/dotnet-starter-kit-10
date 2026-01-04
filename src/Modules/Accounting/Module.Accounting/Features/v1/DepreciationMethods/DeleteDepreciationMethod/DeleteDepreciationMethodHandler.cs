using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.DeleteDepreciationMethod;

public record DeleteDepreciationMethodCommand(Guid Id) : ICommand;

public class DeleteDepreciationMethodHandler(AccountingDbContext context) : ICommandHandler<DeleteDepreciationMethodCommand>
{
    public async ValueTask<Unit> Handle(DeleteDepreciationMethodCommand command, CancellationToken ct)
    {
        var entity = await context.DepreciationMethods.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DepreciationMethod not found");
        
        context.DepreciationMethods.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
