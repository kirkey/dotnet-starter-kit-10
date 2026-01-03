using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.ShareProducts.CreateShareProduct;

public record CreateShareProductCommand(string Name) : ICommand<Guid>;

public class CreateShareProductHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateShareProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateShareProductCommand command, CancellationToken ct)
    {
        var entity = ShareProduct.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ShareProducts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
