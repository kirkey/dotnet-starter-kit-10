using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.CreateShareProduct;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.CreateShareProduct;

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
