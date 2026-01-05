using FluentValidation;
using FSH.Module.Catalog.Contracts.v1.Categories;

namespace FSH.Module.Catalog.Features.v1.Categories.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).ValidateCategoryName();
        RuleFor(x => x.Code).ValidateCategoryCode();
        RuleFor(x => x.Description).ValidateCategoryDescription();
    }
}
