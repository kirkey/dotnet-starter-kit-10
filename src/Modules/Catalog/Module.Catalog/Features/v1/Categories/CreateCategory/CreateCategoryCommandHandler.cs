using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Categories;
using FSH.Module.Catalog.Data;
using Mediator;

namespace FSH.Module.Catalog.Features.v1.Categories.CreateCategory;

/// <summary>
/// Handles the creation of a new category.
/// </summary>
public sealed class CreateCategoryCommandHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        // Create category using domain factory method
        var category = Category.Create(
            command.Name,
            command.Code,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.ParentId);
        
        // Persist to database
        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);
        
        // Return created ID
        return category.Id;
    }
}
