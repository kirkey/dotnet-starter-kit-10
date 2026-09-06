using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Departments;

namespace FSH.Modules.Ai.Features.v1.Departments.DeleteDepartment;

public sealed class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
