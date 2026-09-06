using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Departments;

namespace FSH.Modules.Ai.Features.v1.Departments.UpdateDepartment;

public sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1024).When(x => x.Description is not null);
    }
}
