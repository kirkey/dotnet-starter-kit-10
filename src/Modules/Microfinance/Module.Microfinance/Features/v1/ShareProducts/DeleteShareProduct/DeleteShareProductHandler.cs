using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.DeleteShareProduct;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.DeleteShareProduct;

public class DeleteShareProductHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteShareProductCommand>
{
    public async ValueTask<Unit> Handle(DeleteShareProductCommand command, CancellationToken ct)
    {
        var entity = await context.ShareProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareProduct not found");
        
        context.ShareProducts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
