using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Categories;
using FSH.Module.Catalog.Data;

namespace FSH.Module.Catalog.Features.v1.Categories.CreateCategory;

public sealed class CreateCategoryCommandHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Domain.Category.Create(
            command.Name,
            command.Code,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.ParentCategoryId);

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
