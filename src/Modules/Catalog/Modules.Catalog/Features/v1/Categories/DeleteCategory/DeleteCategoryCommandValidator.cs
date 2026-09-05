using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Categories;

namespace FSH.Modules.Catalog.Features.v1.Categories.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
