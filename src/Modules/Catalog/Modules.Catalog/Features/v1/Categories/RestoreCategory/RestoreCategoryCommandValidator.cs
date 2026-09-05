using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Categories;

namespace FSH.Modules.Catalog.Features.v1.Categories.RestoreCategory;

public sealed class RestoreCategoryCommandValidator : AbstractValidator<RestoreCategoryCommand>
{
    public RestoreCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
