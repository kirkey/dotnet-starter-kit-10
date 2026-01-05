using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Brands;
using FSH.Module.Catalog.Data;

namespace FSH.Module.Catalog.Features.v1.Brands.CreateBrand;

public sealed class CreateBrandCommandHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateBrandCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = Domain.Brand.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.Website);

        context.Brands.Add(brand);
        await context.SaveChangesAsync(cancellationToken);

        return brand.Id;
    }
}
