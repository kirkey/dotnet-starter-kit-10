using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.UpdateShareProduct;

public record UpdateShareProductCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateShareProductHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateShareProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateShareProductCommand command, CancellationToken ct)
    {
        var entity = await context.ShareProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareProduct not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
