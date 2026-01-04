using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.CreateSavingsProduct;

public record CreateSavingsProductCommand(string Name) : ICommand<Guid>;

public class CreateSavingsProductHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateSavingsProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateSavingsProductCommand command, CancellationToken ct)
    {
        var entity = SavingsProduct.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.SavingsProducts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
