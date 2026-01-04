using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.UpdateSavingsProduct;

public record UpdateSavingsProductCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateSavingsProductHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateSavingsProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateSavingsProductCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsProduct not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
