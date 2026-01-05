using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Products;
using FSH.Module.Catalog.Data;
using Mediator;

namespace FSH.Module.Catalog.Features.v1.Products.CreateProduct;

public sealed class CreateProductCommandHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            command.Name,
            command.SKU,
            command.Price,
            command.CategoryId,
            command.BrandId,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.Barcode,
            command.Cost,
            command.QuantityInStock,
            command.Specifications);
        
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }
}
