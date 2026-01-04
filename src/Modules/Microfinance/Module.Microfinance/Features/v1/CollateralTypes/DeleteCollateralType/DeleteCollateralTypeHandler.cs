using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.DeleteCollateralType;

public record DeleteCollateralTypeCommand(Guid Id) : ICommand;

public class DeleteCollateralTypeHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollateralTypeCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollateralTypeCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralTypes.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralType not found");
        
        context.CollateralTypes.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
