using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Departments;

namespace FSH.Modules.Ai.Features.v1.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1024).When(x => x.Description is not null);
    }
}
