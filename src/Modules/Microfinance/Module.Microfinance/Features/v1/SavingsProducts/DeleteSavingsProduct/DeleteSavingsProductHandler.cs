using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.DeleteSavingsProduct;

public record DeleteSavingsProductCommand(Guid Id) : ICommand;

public class DeleteSavingsProductHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteSavingsProductCommand>
{
    public async ValueTask<Unit> Handle(DeleteSavingsProductCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsProduct not found");
        
        context.SavingsProducts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
